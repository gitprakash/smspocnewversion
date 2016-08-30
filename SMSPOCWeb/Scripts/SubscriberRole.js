jqgridDropDownhelper = function (ajaxurl) {
    var listboject = $.ajax({
        url: ajaxurl, async: false,
        success: function (data, result) {
            if (!result)
                alert('Failure to User list');
        }
    }).responseText;

    var lists = eval('(' + listboject + ')');
    var returnlists = '{';
    $(lists).each(function () {
        returnlists += this.Id + ':"' + this.Name + '",';
    });
    returnlists += '}';
    return returnlists;
}


$(document).ready(function myfunction() {
    var usersList = jqgridDropDownhelper("/ManageUserRoles/GetAllUsers");
    var roleList = jqgridDropDownhelper("/Role/GetAllRoles");
    $('#list').jqGrid({
        caption: "Users Role Details",
        url: '/ManageUserRoles/UserRoles/',
        datatype: "json",
        contentType: "application/json; charset-utf-8",
        mtype: 'GET',
        sortname: "Role",
        colNames: ['Id', 'UserName', 'UserList', 'RoleName','RoleList', 'Status'],
        colModel: [
            { name: 'Id', index: 'Id', key: true, hidden: true },
            {
                name: 'UserName',
                index: 'UserName',
                width: 80,
                key: false,
                editable: false 
            },
            {
                name: 'SubscriberId',
                index: 'SubscriberId',
                width: 30,
                key: false,
                hidden: true,
                editable: true,
                editrules: { required: true, edithidden: true },
                editoptions: {
                    value: eval('(' + usersList + ')')
                },
                align: 'center',
                edittype: "select"
            },
              { name: 'RoleName', index: 'RoleName', width: 80, key: false, editable: false },
              {
                  name: 'RoleId',
                  index: 'RoleId',
                  width: 30,
                  key: false,
                  hidden: true,
                  editable: true,
                  editrules: { required: true, edithidden: true },
                  editoptions: {
                      value: eval('(' + roleList + ')')
                  },
                  align: 'center',
                  edittype: "select"
              },
             {
                 name: 'Status', index: 'Status', width: 30, key: false, editable: true, align: 'center', edittype: 'checkbox',
                 editoptions: { value: "Active:InActive" }
             }

        ],
        rowNum: 10,
        rowList: [10, 20, 30],
        viewrecords: true,
        pager: jQuery("#pager"),
        width: '600%',
        height: '100%' 
    });

    jQuery("#list")
        .jqGrid('navGrid',
            '#pager',
            { edit: true, add: true, del: false, refresh: false },
            {
                zIndex: 100,
                url: '/ManageUserRoles/Edit',
                closeOnEscape: true,
                closeAfterEdit: true,
                recreateForm: true,
                beforeShowForm: function(form) {
                    $('#SubscriberId', form).attr('disabled', true);
                    $("#pData, #nData").hide();
                },
                afterSubmit: function(response) {

                    var result = jQuery.parseJSON(response.responseText);
                    if (result.Status === "success") {
                        alert('Successfully modified user roles details');
                        $(this)
                            .jqGrid('setGridParam',
                            { datatype: 'json' })
                            .trigger('reloadGrid');
                        return [true, ''];
                    } else {
                        //error
                        return [false, result.error];
                    }
                }
            },
            {
                zIndex: 100,
                url: "/ManageUserRoles/Add",
                closeOnEscape: true,
                closeAfterAdd: true,
                drag: true,
                afterSubmit: function(response) {

                    var result = jQuery.parseJSON(response.responseText);
                    if (result.Status === "success") {
                        alert('Successfully added user roles details');
                        $(this)
                            .jqGrid('setGridParam',
                            { datatype: 'json' })
                            .trigger('reloadGrid');
                        return [true, ''];
                    } else {
                        return [false, result.error];
                    }
                }

            }
        );
});