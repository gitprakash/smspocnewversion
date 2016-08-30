function mobilenovalidation(value, colname) {
    return /^[789]\d{9}$/.test(value) ?
        [true] :
        [false, "Please enter valid 10 digit mobile number"];
}
var clsarray = {};
$(window).resize(function () {
    var outerwidth = $('#grid').width();
    $('#list').setGridWidth(outerwidth); // setGridWidth method sets a new width to the grid dynamically
});

$(document).ready(function myfunction() {

    //loading template drodown
    $("#btnexportExcel").hide();

    $('#list').jqGrid({
        caption: "Student Details",
        url: '/Contact/Index',
        datatype: "json",
        contentType: "application/json; charset-utf-8",
        mtype: 'GET',
        sortname: "Name",
        colNames: ['Id', 'RollNo', 'Name', 'Class', 'Class List', 'Section', 'Section List', 'Mobile', 'BloodGroup', 'Status'],
        colModel: [
               { name: 'Id', index: 'Id', key: true, hidden: true },
              {
                  name: 'RollNo', index: 'RollNo', width: 40, key: false, editable: true, align: 'center',
                  editrules: { required: true }, searchoptions: { sopt: ['eq', 'ne'] }
              },
              {
                  name: 'Name', index: 'Name', width: 70, key: false, editable: true, align: 'center',
                  editoptions: { size: 100, minlength: 2 }, editrules: { required: true }, searchoptions: { sopt: ['eq', 'ne'] }
              },
              {
                  name: 'Class', index: 'Class', width: 30, key: false, editable: false, align: 'center', editrules: { required: false }, searchoptions: { sopt: ['eq', 'ne'] }
              },
              {
                  name: 'SubscriberStandardId', index: 'SubscriberStandardId', width: 30, key: false, hidden: true, editable: true, editrules: { required: true, edithidden: true },
                  align: 'center', edittype: "select",
                  editoptions: {
                      value: eval('(' + standarList + ')'),
                      dataInit: function (elem) {
                          LoadSectionList($(elem).val());
                      },
                      dataEvents: [
                           { type: 'change', fn: function (e) { changeSectionlist($(e.target).val(), e.target); } }
                      ]
                  }
              },
              {
                  name: 'Section', index: 'Section', width: 30, key: false, editable: false, align: 'center',
                  edittype: 'select', searchoptions: { sopt: ['eq', 'ne'] }
              },
              {
                  name: 'SubscriberStandardSectionId', index: 'SubscriberStandardSectionId', width: 30, key: false, hidden: true,
                  editable: true, editrules: { edithidden: true },
                  align: 'center', edittype: "select"
              },
              {
                  name: 'Mobile', index: 'Mobile', width: 70, key: false, editable: true, align: 'center',
                  editrules: { required: true, custom: true, custom_func: mobilenovalidation },searchoptions:{sopt:['eq','ne']}
              },
              { name: 'BloodGroup', index: 'BloodGroup', width: 30, key: false, editable: true, align: 'center', searchoptions: { sopt: ['eq', 'ne'] } },

             { name: 'Status', index: 'Status', width: 30, key: false, editable: true, align: 'center',search:false, edittype: 'checkbox', editoptions: { value: "Active:InActive" } }


        ],
        rowNum: 20,
        rowList: [10, 20, 30],
        jsonReader: { id: "0" },
        viewrecords: true,
        pager: jQuery("#pager"),
        autowidth: true,
        shrinkToFit: true,
        gridComplete: function ()
        {
            $("#btnexportExcel").show();
        }
    });

    jQuery("#list").jqGrid('navGrid', '#pager', {
        edit: true, edittitle: 'Edit Student', add: true, addtitle: 'Add Student', del: true, deltitle: 'Delete Student', refresh: false
    },
        {
            width: 400,
            zIndex: 100,
            url: '/Contact/Edit',
            closeOnEscape: true,
            closeAfterEdit: true,
            recreateForm: true,
            afterSubmit: function (response) {
                var result = jQuery.parseJSON(response.responseText);
                if (result.Status === "success") {
                    alert('Successfully Update Student details');
                    $(this).jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
                    return [true, ''];
                }
                else {
                    return [false, result.error];
                }
            }
        },
       {
           width: 400,
           zIndex: 100,
           url: "/Contact/Add",
           closeOnEscape: true,
           closeAfterAdd: true,
           drag: true,
           editData: {
               Id: 0
           },
           afterSubmit: function (response) {

               var result = jQuery.parseJSON(response.responseText);
               if (result.Status === "success") {
                   alert('Successfully added Student details');
                   $(this).jqGrid('setGridParam',
                     { datatype: 'json' }).trigger('reloadGrid');
                   return [true, ''];
               }
               else {
                   return [false, result.error];
               }
           }

       },
       {
           zIndex: 100,
           url: "/Contact/Delete",
           closeOnEscape: true,
           closeAfterDelete: true,
           drag: true,
           afterSubmit: function (response) {
               var result = jQuery.parseJSON(response.responseText);
               if (result.Status == "success") {
                   alert('Successfully Deleted Student details');
                   $(this).jqGrid('setGridParam',
                     { datatype: 'json' }).trigger('reloadGrid');
                   return [true, ''];
               }
               else {
                   return [false, result.error];
               }
           }

       },
       {
           closeOnEscape: true,
           closeAfterSearch: true,
           closeAfterReset: true
       }

        );
});

var data = $.ajax({
    url: '/Contact/GetStandards',
    dataType: 'json', async: false,
    success: function (data, result) {
        if (!result) alert('Failure to retrieve the ContactList related lookup data.');
    }
}).responseText;
var Standards = eval('(' + data + ')');

var standarList = '{';
$(Standards).each(function () {
    standarList += this.SubscriberStandardId + ':"' + this.StandardName + '",';
});
standarList += '}';

function LoadSectionList(standardid) {
    if (standardid) {
        var sections = GetSectionList(standardid);
        var sectionsList = '{';
        $(sections).each(function (i, val) {
            sectionsList += val.SubscriberStandardSectionId + ':"' + val.SectionName + '",';
        });
        sectionsList += '}';
        if (sections) {
            $("#list").jqGrid('setColProp', 'SubscriberStandardSectionId', { editoptions: { value: eval('(' + sectionsList + ')') } });
        }
    }
}

function GetSectionList(standardid) {
    var data = $.ajax({
        url: '/Contact/GetSections',
        data: { subscriberStandardId: standardid },
        dataType: 'json', async: false,
        success: function (data, result) {
            if (!result) alert('Failure to retrieve the sectionlist related lookup data.');
        }
    }).responseText;
    var sections = eval('(' + data + ')');
    return sections;
}

function changeSectionlist(standardid, StandardElement) {
    if (standardid) {
        var sections = GetSectionList(standardid),
        newOptions = '';
        $(sections).each(function (i, val) {
            newOptions += '<option role="option" value="' + val.SubscriberStandardSectionId + '">' +
                   val.SectionName + '</option>';
        });
        if ($(StandardElement).is('.FormElement')) {
            // form editing
            $(StandardElement).closest('form.FormGrid').find("select#SubscriberStandardSectionId.FormElement").html(newOptions);
        } else {
            // inline editing
            $row = $(StandardElement).closest('tr.jqgrow');
            $("select#" + $.jgrid.jqID($row.attr('id')) + "_SubscriberStandardSectionId").html(newOptions);
        }
    }
}