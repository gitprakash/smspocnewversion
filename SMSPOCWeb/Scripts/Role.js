$(document).ready(function myfunction() {

    $('#list').jqGrid({
        caption: "Role Details",
        url: '/Role/Index/',
        datatype: "json",
        contentType: "application/json; charset-utf-8",
        mtype: 'GET',
        sortname:"Name",
        colNames: ['Id', 'Name', "Description","Created By","Created Date"],
        colModel: [
              { name: 'Id', index: 'Id',  key: true,hidden:true},
              { name: 'Name', index: 'Name', width: 80, key: false, editable: true },
              { name: 'Description', index: 'Description', width: 80, key: false, editable: true },
              { name: 'CreatedBy', index: 'CreatedBy', width: 80, key: false, editable: false },
               { name: 'CreatedDate', index: 'CreatedDate', formatter: 'date', width: 80, key: false, editable: false }
        ],
        rowNum: 10,
        rowList: [10, 20, 30],
        viewrecords: true,
        pager: jQuery("#pager"),
        autowidth: true,
        height: '100%',
    });

    jQuery("#list").jqGrid('navGrid', '#pager', { edit: true, add: true, del: true, refresh: true },
        {
            zIndex: 100,
            url: '/Role/Edit',
            closeOnEscape: true,
            closeAfterEdit: true,
            recreateForm: true,
            afterComplete: function (response) {
                if (response.responseText) {
                    alert(response.responseText);
                }
            }
        },
        {
            zIndex: 100,
            url: "/Role/Create",
            closeOnEscape: true,
            closeAfterAdd: true,
            recreateForm: true,
            afterComplete: function (response) {
                if (response.responseText) {
                    alert(response.responseText);
                }
            }
        },
        {
            zIndex: 100,
            url: "/Role/Delete",
            closeOnEscape: true,
            closeAfterDelete: true,
            recreateForm: true,
            msg: "Are you sure you want to delete Role... ? ",
            afterComplete: function (response) {
                if (response.responseText) {
                    alert(response.responseText);
                }
            }
        });
});