var helpers =
{
    buildDropdown: function (result, dropdown, emptyMessage) {
        // Remove current options
        dropdown.html('');
        // Add the empty option with the empty message
        dropdown.append('<option value="">' + emptyMessage + '</option>');
        // Check result isnt empty
        if (result !== '') {
            // Loop through each of the results and append the option to the dropdown
            $.each(result, function (k, v) {
                dropdown.append('<option value="' + v.Description + '">' + v.Name + '</option>');
            });
        }
    }
}

function adjustamodal(cntrolid) {
    var altura = $(window).height() - 205; //value corresponding to the modal heading + footer
    $("#" + cntrolid).css({ "height": altura, "overflow-y": "auto" });
}

$(window).resize(function () {
    var outerwidth = $('#grid').width();
    $('#list').setGridWidth(outerwidth); // setGridWidth method sets a new width to the grid dynamically
    //modal height scrollbar
    adjustamodal("divmodalstudentlist");
});

function GetSMSMessageCount(value) {
    var chars = value.length,
            messages = Math.ceil(chars / 160),
    remaining = messages * 160 - (chars % (messages * 160) || messages * 160);
    return { 'messages': messages, 'remaining': remaining };
}

$(document).ready(function () {
    var $remaining = $('#remaining'),
    $messages = $remaining.next();
    $('#txtsms').keyup(function () {
        var messagesremaining = GetSMSMessageCount(this.value.trim()),
          remaining = messagesremaining.remaining;
        messages = messagesremaining.messages;
        $remaining.text(remaining + ' characters remaining');
        $messages.text(messages + ' message(s)');
    });

    $('#StudentModal').on('show.bs.modal', function (e) {
        // do something...
        $(this).find('.modal-dialog').addClass('modal-lg')
        ConstructStudentJqGrid();
    })
    $('#TemplateModal').on('show.bs.modal', function (e) {
        // do something...
        $(this).find('.modal-dialog').addClass('modal-lg')
        ConstructTemplateJqGrid();
    })
    $('#btnstudentconfirm').on('click', function (e) {
        // do something...
        ProcessSelectedStudent();
    })
    $('#btntemplateconfirm').on('click', function (e) {
        // do something...
        ProcessSelectedTemplate();
    })
    LoadSubscriberSMS(fnsuccess, true);
    // adjustamodal();
});

var selectedcontactarray = [];
$(function () {

    $("#btnclear").on('click', function () {
        $("#ContactList").empty();
        $("#txtsms").val("");
    });

    $('#btnsendconfirm').on('click', function (e) {
        var $btn = $(this).button('loading');
        // business logic... 
        SendMessageAjaxRequest();
    });
    $('#dropdown').change(function () {
        $('#txtsms').val($(this).val());
        $('#txtsms').keyup();
    });
    $('#submitMyForm')
        .click(function () {
            $("#divmodalmessage").removeAttr('style');
            $('#ErrorResultArea').empty();
            $('#SuccessResultArea').empty();
            $("#btnsendconfirm").button('reset');
            $('#ConfirmModal').modal({ backdrop: 'static', keyboard: false })
            $('#ConfirmModal').modal('show');
            if (validatemesssageinputs()) {
                $("#divuploadconfirm").show();
            }
            else {
                $("#divuploadconfirm").hide();
            }

        });

    var validatemesssageinputs = function () {
        var status = true;
        if (selectedcontactarray.length === 0) {
            $("<div class='label-danger'>Please select a contact</div>").appendTo("#ErrorResultArea");
            status = false;
        }
        if ($('#txtsms').val().trim().length === 0) {
            $("<div class='label-danger'>Message cannot be blank</div>").appendTo("#ErrorResultArea");
            status = false;

        }
        return status;
    }
    var SendMessageAjaxRequest = function () {
        var count = GetSMSMessageCount($('#txtsms').val().trim());
        $.ajax({
            type: 'Post',
            url: '/Notify/SendMessage',
            data: { messageViewModel: selectedcontactarray, Message: $('#txtsms').val().trim(), messagecount: count.messages },
            success: function (data) {
                if (data.Status === true) {
                    buildsuccesstable(data.SuccessResult);
                    LoadSubscriberSMS(fnsuccess, false);
                    $("#btnsendconfirm").button('reset');
                }
                if (data.Status === false) {
                    $("<div class='danger'>Error Occured " + data.ErrorResult + "</div>").appendTo("#ErrorResultArea");
                }
            },
            error: function (data, error) {
                $("<div class='danger'>problem in sending message" + data + "</div>").appendTo("#ErrorResultArea");
            }
        });
    }
    $('#ContactList').on('click', '.close', function () {
        var id = $(this).attr('contactid');
        $('#' + id).remove();
        $(this).remove();
        $.each(selectedcontactarray, function (i, contact) {
            if (selectedcontactarray[i]) {
                if (selectedcontactarray[i].Id === id) // delete index
                {
                    selectedcontactarray.splice(i, 1);
                    return false;
                }
            }
        });
        return false;
    });
});

function CreateLinks() {
    $("#ContactList").empty();
    $.each(selectedcontactarray, function (i, contact) {
        var span = $("<span  contactid=" + contact.Id + " class='close'> x <span>").css({ 'padding': '2px 5px', 'background': '#ccc', 'color': 'red' });
        var a = $("<a href='javascript:void(0)'></a>")
            .css({ 'display': 'inline-block' })
            .append(span);
        $("<span id=" + contact.Id + " class='btn btn-group btn-info btn-xs' title= " + contact.Name + ":" + contact.Mobile + ">" + contact.RollNo + "</span>").css('margin-right', '10px').appendTo("#ContactList");
        $(a).appendTo('#ContactList');
    });
}


function ConstructTemplateJqGrid() {
    $('#templatelist').jqGrid({
        caption: "Template Details",
        url: '/Template/Index',
        datatype: "json",
        contentType: "application/json; charset-utf-8",
        mtype: 'GET',
        sortname: "Name",
        colNames: ['Id', 'Name', 'Description'],
        colModel: [
              { name: 'Id', index: 'Id', key: true, hidden: true },
              { name: 'Name', index: 'Name', width: 220, key: false, align: 'center' },
              { name: 'Description', index: 'Description', width: 520, key: false, align: 'center' },

        ],
        rowNum: 10,
        rowList: [10, 20, 50, 100],
        viewrecords: true,
        pager: jQuery("#templatepager"),
        multiselect: true,
        gridview: true,
        shrinkToFit: true,
        autowidth: true
    });
    jQuery("#templatelist").jqGrid('navGrid', '#templatepager', { edit: false, add: false, del: false, search: false });
    // jQuery("#list").jqGrid('filterToolbar', { searchOperators: true });
    jQuery("#templatelist").jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false, searchOperators: true });
    // adjustamodal("divmodalstudentlist");
}

function ConstructStudentJqGrid() {
    $('#list').jqGrid({
        caption: "Student Details",
        url: '/Contact/Index',
        datatype: "json",
        contentType: "application/json; charset-utf-8",
        mtype: 'GET',
        sortname: "Name",
        colNames: ['Id', 'RollNo', 'Name', 'Class', 'Section', 'Mobile'],
        colModel: [
              { name: 'Id', index: 'Id', key: true, hidden: true },
              { name: 'RollNo', index: 'RollNo', width: 100, key: false, align: 'center', searchoptions: { sopt: ['eq'] } },
              { name: 'Name', index: 'Name', width: 200, key: false, align: 'center', searchoptions: { sopt: ['eq'] } },
              { name: 'Class', index: 'Class', width: 70, key: false, align: 'center', searchoptions: { sopt: ['eq'] } },
              { name: 'Section', index: 'Section', width: 100, key: false, align: 'center', searchoptions: { sopt: ['eq'] } },
              { name: 'Mobile', index: 'Mobile', width: 180, key: false, align: 'center', searchoptions: { sopt: ['eq'] } }

        ],
        rowNum: 15,
        rowList: [10, 15, 20, 50, 100],
        viewrecords: true,
        pager: jQuery("#pager"),
        multiselect: true,
        gridview: true,
        shrinkToFit: true,
        autowidth: true
    });
    jQuery("#list").jqGrid('navGrid', '#pager', { edit: false, add: false, del: false, search: true }, {}, {}, {},
{
    multipleSearch: false, closeAfterSearch: true, closeAfterReset: true,
    onClose: function () {
        var ofilter = $("#list").getGridParam("postData");
        //for (var i = 0; i < ofilter.rules.length; i++) {
        //    alert(ofilter.rules[i].field); //- field name 
        //    alert(ofilter.rules[i].data); //- value   
        //    alert(ofilter.rules[i].op); //- which operation performed  
        //}
        return true; // return true to close the search grid
    }
});
    // jQuery("#list").jqGrid('filterToolbar', { searchOperators: true });
    //jQuery("#list").jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false, searchOperators: true });
    adjustamodal("divmodalstudentlist");
}

var buildsuccesstable = function (data) {
    //Crate table html tag
    $("<button class='btn btn-primary active pull-right'>Message Processed to Send <span class='badge label-primary pull-right'> " + data.length + " </span></button>").appendTo("#SuccessResultArea");
    var table = $("<table id=successtable  class='table table-hour table-bordered table-responsive'></table>").appendTo("#SuccessResultArea");
    //Create table header row
    var rowHeader = $("<tr class='info'></tr>").appendTo(table);
    $("<th></th>").text("RollNo").appendTo(rowHeader);
    $("<th></th").text("Name").appendTo(rowHeader);
    $("<th></th").text("MobileNo").appendTo(rowHeader);
    $("<th></th").text("Message Status").appendTo(rowHeader);
    $.each(data, function (i, value) {
        //Create new row for each record
        var textclass = (value.SentStatus == true ? "text-success" : "text-danger");
        var row = $("<tr class=" + textclass + "></tr>").appendTo(table);
        $("<td></td>").text(value.RollNo).appendTo(row);
        $("<td></td>").text(value.Name).appendTo(row);
        $("<td></td>").text(value.Mobile).appendTo(row);
        $("<td></td>").text(value.SentStatus == true ? "Sent" : "Not Sent" + value.MessageError).appendTo(row);
    });
    adjustamodal("divmodalmessage");
    $("#divuploadconfirm").hide();
};

function fnsuccess(data) {
    $("#opensmscnt").html(data.Openingsms);
    $("#smsbalcnt").html(data.balancesms);
}
var ProcessSelectedTemplate = function () {
    var ids = $("#templatelist").jqGrid('getGridParam', 'selarrrow');
    var selectedtemplate = ids.length;
    if (selectedtemplate === 1) {
        $.each(ids,
            function (i, rowid) {
                var Message = $('#templatelist').jqGrid('getCell', rowid, 'Message');
                var Description = $('#templatelist').jqGrid('getCell', rowid, 'Description');
                $('#txtsms').val(Description);
                $('#txtsms').keyup();
            });
        $('#TemplateModal').modal('hide')
    }
    else if (selectedtemplate === 0) {
        alert('please select a atleast one template');
        return false;
    }
    else if (selectedtemplate > 1) {
        alert('please select only one template');
        return false;
    }
};
var ProcessSelectedStudent = function () {
    var ids = $("#list").jqGrid('getGridParam', 'selarrrow');
    if (ids.length > 0) {

        selectedcontactarray = [];
        $.each(ids,
            function (i, rowid) {
                var name = $('#list').jqGrid('getCell', rowid, 'Name');
                var rollno = $('#list').jqGrid('getCell', rowid, 'RollNo');
                var standard = $('#list').jqGrid('getCell', rowid, 'Class');
                var section = $('#list').jqGrid('getCell', rowid, 'Section');
                var mobile = $('#list').jqGrid('getCell', rowid, 'Mobile');
                var contactvm = {
                    'Id': rowid,
                    'Name': name,
                    'RollNo': rollno,
                    'Standard': standard,
                    'Section': section,
                    'Mobile': mobile
                };
                if (rowid) {
                    selectedcontactarray.push(contactvm);
                }
            });
        CreateLinks();
        $('#StudentModal').modal('hide')
    } else {
        alert('please select a student');
        return false;
    }
};