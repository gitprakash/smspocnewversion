
$(window).resize(function () {
    var outerwidth = $('#grid').width();
    $('#list').setGridWidth(outerwidth); // setGridWidth method sets a new width to the grid dynamically
});
$.extend($.jgrid.nav, {editicon: "ui-icon-customedit" , delicon: "ui-icon-customtrash" });
function fnsuccess(data) {
    $("#opensmscnt").html(data.Openingsms);
    $("#smsbalcnt").html(data.balancesms);
}
var lastsel;
LoadSubscriberSMS(fnsuccess, true);

$(document).ready(function myfunction() {
    $('#list').jqGrid({
        caption: "Sent Message Status",
        url: '/Notify/GetMessageHistory',
        datatype: "json",
        contentType: "application/json; charset-utf-8",
        mtype: 'GET',
        sortname: "SentDateTime",
        colNames: ['Id', 'RollNo', 'Name', 'Class', 'Section', 'Message', 'MobileNo', 'Status', 'SentDateTime'],
        colModel: [
               { name: 'Id', index: 'Id', key: true, hidden: true },
              {
                  name: 'RollNo', index: 'RollNo', width: 30, key: false, editable: false, align: 'center',
                  editrules: { required: true }
              },
              {
                  name: 'Name', index: 'Name', width: 40, key: false, editable: false, align: 'center'
              },
              {
                  name: 'Class', index: 'Class', width: 10, key: false, editable: false, align: 'center'
              },
               {
                   name: 'Section', index: 'Section', width: 20, key: false, editable: false, align: 'center'
               },
               {
                   name: 'Message', index: 'Message', width: 80, key: false, editable: true, align: 'center', edittype: 'textarea', editoptions: { rows: 4, cols: 80 }
               },
               {
                   name: 'MobileNo',
                   index: 'MobileNo',
                   width: 30,
                   key: false
               },
                { name: 'Status', index: 'Status', width: 20, key: false, align: 'center' }
                 ,
                    {
                        name: 'SentDateTime',
                        index: 'SentDateTime',
                        formatter: 'date',
                        formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y h:i:s A" },
                        width: 30,
                        key: false
                    }
        ],
        rowNum: 20,
        rowList: [10, 20, 30],
        jsonReader: { id: "0" },
        viewrecords: true,
        pager: jQuery("#pager"),
        autowidth: true,
        shrinkToFit: true,
        beforeSelectRow: function (rowid) {
            var status = $('#list').jqGrid('getCell', rowid, 'Status');
            if (status !== "Sent") {
                // eneble the "Edit" button in the navigator
                $("#edit_" + this.id).removeClass('ui-state-disabled');
                $("#del_" + this.id).removeClass('ui-state-disabled');
            }
            else {
                $("#edit_" + this.id).addClass('ui-state-disabled');
                $("#del_" + this.id).addClass('ui-state-disabled');
            }
            return true;
        }
    });
    $("#list").jqGrid('navGrid', '#pager', { add: false, edit: true, del: true, refresh: true, edittitle: 'Edit Message', deltitle: 'SendAgain' },
    {
        width:400,
        url: "/Notify/ReSend",
        closeOnEscape: true,
        closeAfterEdit: true,
        drag: true,
        recreateform: true,
        bSubmit: "Send Message",
        editData: {
            sipcId: function () {
                var selectedRowId = $('#list').jqGrid('getGridParam', 'selrow');
                return $('#list').jqGrid('getCell', selectedRowId, 'sipcId');
            }
        },
        afterSubmit: function (response, postdata) {
            //responsetext 1 and 0 ajax success or else its a error
            if (response.responseText == "1" || response.responseText == "0") {
                alert('Successfully updated Wintails details');
                LoadSubscriberSMS(fnsuccess, true);
                $(this).jqGrid('setGridParam',
                  { datatype: 'json' }).trigger('reloadGrid');
                return [true, '']
            }
            else {
                //error
                return [false, response.responseText]
            }
        }
    }, {},
    {
        caption: "Send Again",
        msg: "Send selected record(s)?",
        url: "/Notify/ReSend",
        closeOnEscape: true,
        closeAfterAdd: true,
        recreateform: true,
        bSubmit: "Send Message",
        editData: { 
        },
        afterSubmit: function (response, postdata) {
            var result = JSON.parse(response.responseText);
            //responsetext 1 and 0 ajax success or else its a error
            if (result.Status == "success") {
                alert('Messing has been sent');
                LoadSubscriberSMS(fnsuccess, true);
                $(this).jqGrid('setGridParam',
                  { datatype: 'json' }).trigger('reloadGrid');
                return [true, '']
            }
            else {
                //error
                alert('Messing sending failed, try again');
                return [false, response.responseText]
            }
        }
    }
    );
});