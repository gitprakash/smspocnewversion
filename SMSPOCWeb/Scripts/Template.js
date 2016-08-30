
$(window).resize(function () {
    var outerwidth = $('#grid').width();
    $('#list').setGridWidth(outerwidth); // setGridWidth method sets a new width to the grid dynamically
});


$(document).ready(function myfunction() {

    $('#list').jqGrid({
        caption: "Template Details",
        url: '/Template/Index',
        datatype: "json",
        contentType: "application/json; charset-utf-8",
        mtype: 'GET',
        sortname: "Name",
        colNames: ['Id', 'Name', 'Description', 'Status'],
        colModel: [
               { name: 'Id', index: 'Id', key: true, hidden: true },
              {
                  name: 'Name', index: 'RollNo', width: 60, key: false, editable: true, align: 'center',
                  editrules: { required: true, cols:30 }
              },
              {
                  name: 'Description', index: 'Description', width: 120, key: false,edittype:'textarea', editable: true, align: 'center',
                  editoptions: { rows:8,cols:60}, editrules: { required: true }
              },
        {
            name: 'Status',
            index: 'Status',
            width: 30,
            key: false,
            editable: true,
            align: 'center',
            edittype: 'checkbox',
          
            editoptions: { value: "Active:InActive", defaultValue: "Active" }
             }


        ],
        rowNum: 10,
        rowList: [10, 20, 30],
        jsonReader: { id: "0" },
        viewrecords: true,
        pager: jQuery("#pager"),
        autowidth: true,
        shrinkToFit: true

    });

    jQuery("#list").jqGrid('navGrid', '#pager', { edit: true, edittitle: 'Edit Template', add: true, addtitle: 'Add Template', del: true, deltitle: 'Delete Template', refresh: false },
        {
            width: 400,
            zIndex: 100,
            url: '/Template/Edit',
            closeOnEscape: true,
            closeAfterEdit: true,
            recreateForm: true,
            afterSubmit: function (response) {
                var result = jQuery.parseJSON(response.responseText);
                if (result.Status === "success") {
                    alert('Successfully Update Template details');
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
           url: "/Template/Add",
           closeOnEscape: true,
           closeAfterAdd: true,
           drag: true,
           editData: {
               Id: 0
           },
           afterSubmit: function (response) {

               var result = jQuery.parseJSON(response.responseText);
               if (result.Status === "success") {
                   alert('Successfully added Template details');
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
           url: "/Template/Delete",
           closeOnEscape: true,
           closeAfterDelete: true,
           drag: true,
           afterSubmit: function (response) {
               var result = jQuery.parseJSON(response.responseText);
               if (result.Status == "success") {
                   alert('Successfully Deleted Template details');
                   $(this).jqGrid('setGridParam',
                     { datatype: 'json' }).trigger('reloadGrid');
                   return [true, ''];
               }
               else {
                   return [false, result.error];
               }
           }

       }

        );
});