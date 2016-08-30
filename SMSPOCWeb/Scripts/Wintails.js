$(document).ready(function myfunction() {
    var lastSel;
    $('#list').jqGrid({
        caption: "Store Details",
        url: "/WintailsStore/GetWintailsStores",
        datatype: "json",
        contentType: "application/json; charset-utf-8",
        mtype: 'GET',
        colNames: ['sipcId', 'Store No', 'Store  Name', 'Country', 'Wintails Report', 'IP Address', 'siPCName', 'Id'],
        colModel: [
                        { key: false, name: 'sipcId', index: 'sipcId', width: 50, hidden: true },
                        {
                            key: false, name: 'deptID', index: 'deptID', width: 100, align: "center", editable: true, editrules: { required: true },
                            searchoptions: { sopt: ['eq'] }, editoptions: {
                                size: 10, maxlengh: 10,
                                dataInit: function (element) {
                                    $(element).keyup(function () {
                                        var val1 = element.value;
                                        var num = new Number(val1);
                                        if (isNaN(num))
                                        { alert("Please enter a valid number"); }
                                    });
                                }
                            }
                        },
                        {
                            key: false, name: 'deptName', index: 'deptName', width: 200, align: "center", editable: true,
                            editrules: { required: true }, editoptions: { size: 25 }, searchoptions: { sopt: ['eq'] }
                        },
                        { key: false, name: 'ctryName', index: 'ctryName', width: 150, align: "center", editable: false },
                        {
                            name: "WintailsFlag", index: "WintailsFlag", width: 130, search: false,
                            editable: false,
                            edittype: 'checkbox', editoptions: { value: "True:False" },
                            formatter: "checkbox", formatoptions: { disabled: true },
                            align: "center"
                        },
                        {
                            key: false, name: 'siIPAddress', index: 'siIPAddress', width: 150, align: "center", editable: true,
                            editrules: { required: true, custom: true, custom_func: CheckIPaddress }, searchoptions: { sopt: ['eq'] }
                        },
                        { key: false, name: 'siPCName', index: 'siPCName', width: 100, hidden: true, editable: true, viewable: true, editrules: { edithidden: true, required: true }, searchoptions: { sopt: ['eq'] } },
                        { key: true, name: 'Id', index: 'Id', width: 50, hidden: true },
        ],
        pager: $('#pager'),
        rowList: [10, 20, 30],
        height: '100%',
        width: '800%',
        viewrecords: true,
        emptytrecords: 'No records to display',
        jsonReader: {
            root: "rows",
            page: "page",
            total: "total",
            record: "records",
            repeatitems: false,
            id: "0"
        },
        rowNum: 10,
        sortname: 'deptID',
        sortorder: 'asc',
        beforeSelectRow: function (rowid) {
            var WintailsFlagvalue = $('#list').jqGrid('getCell', rowid, 'WintailsFlag');
            if (WintailsFlagvalue == "False") {
                // eneble the "Edit" button in the navigator
                $("#edit_" + this.id).addClass('ui-state-disabled');
                $("#del_" + this.id).addClass('ui-state-disabled');
            }
            else {
                $("#edit_" + this.id).removeClass('ui-state-disabled');
                $("#del_" + this.id).removeClass('ui-state-disabled');
            }
            return true;
        }
    });
    jQuery("#list").jqGrid('navGrid', '#pager', {
        cloneToTop: true,
        add: true,
        edit: true,
        refresh: false,
        addtext: 'Add',
        edittext: 'Edit',
        deltext: 'Delete',
        searchtext: 'Search'

    },
     {
         url: "/WintailsStore/Edit",
         closeOnEscape: true,
         closeAfterEdit: true,
         drag: true,
         recreateform: true,
         beforeShowForm: function (form) {
             $('#deptID', form).attr('readOnly', true);
             $('#ctryName', form).attr('readOnly', true);
             $("#pData, #nData").hide();
         },
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
                 $(this).jqGrid('setGridParam',
                   { datatype: 'json' }).trigger('reloadGrid');
                 return [true, '']
             }
             else {
                 //error
                 return [false, response.responseText]
             }
         }
     }, // edit options
                  {
                      url: "/WintailsStore/Add",
                      closeOnEscape: true,
                      closeAfterAdd: true,
                      drag: true,
                      beforeShowForm: function (form) {
                          var selectedRowId = $('#list').jqGrid('getGridParam', 'selrow');
                          if (selectedRowId) {
                              var deptidvalue = $('#list').jqGrid('getCell', selectedRowId, 'deptID');
                              var deptNamevalue = $('#list').jqGrid('getCell', selectedRowId, 'deptName');
                              var ctryNamevalue = $('#list').jqGrid('getCell', selectedRowId, 'ctryName');
                              $('#deptID', form).val(deptidvalue);
                              $('#deptName', form).val(deptNamevalue);
                              $('#ctryName', form).val(ctryNamevalue);
                              $("#pData, #nData").hide();
                          }
                      },
                      
                      afterSubmit: function (response, postdata) {
                         
                          if (response.responseText == "1" || response.responseText == "0") {
                              alert('Successfully added Wintails details');
                              $(this).jqGrid('setGridParam',
                                { datatype: 'json' }).trigger('reloadGrid');
                              return [true, '']
                          }
                          else {
                             
                              return [false, response.responseText]
                          }
                      }

                  }, // add options
                  {
                      url: "/WintailsStore/Delete",
                      closeOnEscape: true,
                      closeAfterAdd: true,
                      serializeDelData: function (postdata) {
                          var selectedRowId = $('#list').jqGrid('getGridParam', 'selrow');
                          var sipcid = $('#list').jqGrid('getCell', selectedRowId, 'sipcId');
                          // append postdata with any information 
                          return { id: sipcid, oper: postdata.oper };
                      },
                      afterSubmit: function (response, postdata) {
                          if (response.responseText == "1" || response.responseText == "0") {
                              alert('Successfully deleted Wintails details');
                              $(this).jqGrid('setGridParam',
                                { datatype: 'json' }).trigger('reloadGrid');
                              return [true, '']
                          }
                          else {
                              return [false, response.responseText]
                          }
                      }
                  },//del options

                  {
                      closeOnEscape: true,
                      closeAfterSearch: true,
                      closeAfterReset: true
                  }, // search options
                  {   // vew options

                  }
        );

});

function CheckIPaddress(inputText) {
    var ipformat = /^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;
    if (inputText.match(ipformat)) {
        return [true, ""];
    }
    else {
        return [false, "Please enter valid IP Address"];
    }
}
