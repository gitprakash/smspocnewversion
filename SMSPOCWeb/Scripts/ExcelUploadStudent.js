$(window).resize(function () {
    adjustmodal();
});

function adjustmodal() {
    var altura = $(window).height() - 205; //value corresponding to the modal heading + footer
    $(".resize-scroll").css({ "height": altura, "overflow-y": "auto" });
}

var IsExcelFile = function (inputfilename) {
    var ext = inputfilename.match(/\.(.+)$/)[1];
    switch (ext) {
        case 'xls':
        case 'xlsx':
            return true;
            break;
        default:
            return false;
    }
}

$(document).ready(function () {
    $('#btnupload').hide();

    $('INPUT[type="file"]').change(function () {
        if ($(this).val().trim() != "") {
            var istrue = IsExcelFile($(this).val().trim());
            if (istrue) {
                $('#btnupload').show();
            }
            else {
                $("<div class='alert-danger'>Please select a excell file (xls,xlsx)</div>").appendTo("#divmodalvalidate");
                $("#bsmodalinputfile").modal('show');
            }
        }
        else {
            $('#btnupload').show();
        }
    });

    $('#btn-reset').on('click', function (e) {
        var $el = $('#dataFile');
        $el.wrap('<form>').closest('form').get(0).reset();
        $el.unwrap();
    });

    $('#btnstudentconfirm').on('click', function (e) {
        var $btn = $(this).button('loading')
        // business logic...
        $("#bulkuploadstudentform").submit();
    });
    $('#btnupload').on('click', function (e) {
        $("#divmodalvalidate").empty();
        var $el = $('#dataFile').val().trim();
        if ($el === "") {
            $("<div class='alert-danger'>Please select File</div>").appendTo("#divmodalvalidate");
            $('#bsmodalinputfile').modal({ backdrop: 'static', keyboard: false })
            $("#bsmodalinputfile").modal('show');
        }
        else if (!IsExcelFile($el)) {
            $("<div class='alert-danger'>Please select a excell file (xls,xlsx)</div>").appendTo("#divmodalvalidate");
            $('#bsmodalinputfile').modal({ backdrop: 'static', keyboard: false })
            $("#bsmodalinputfile").modal('show');

        }
        else {
            $(".resize-scroll").removeAttr('style');
            $("#ErrorResultArea").empty();
            $("#SuccessResultArea").empty();
            $("#btnstudentconfirm").show();
            $("#divuploadconfirm").show();
            $('#StudentModal').modal({ backdrop: 'static', keyboard: false })
            $('#StudentModal').modal('show');
        }
    });

    $('#bulkuploadstudentform').submit(function (e) {
        e.preventDefault();
        var data = new FormData(this); // <-- 'this' is your form element 
        $.ajax({
            url: '/Contact/SaveExcelFile',
            data: data,
            cache: false,
            contentType: false,
            processData: false,
            type: 'POST',
            success: function (data, status) {
                $("#btnstudentconfirm").button('reset');
                $("#divuploadconfirm").hide();
                $("#btnstudentconfirm").hide();
                $("#dataFile").val('');
                if (data.Status === false && data.ErrorResult) {
                    builderrortable(data.ErrorResult);
                }
                else if (data.Status === true && data.SuccessResult) {
                    buildsuccesstable(data.SuccessResult);
                }
            },
            error: function (e, details, xhr) {
                
            }
        });
    });

    var builderrortable = function (data) {
        //Crate table html tag
        $("<div class='text-info'>Error occured </div>").appendTo("#ErrorResultArea");
        var table = $("<table id=DynamicTable  class='table table-bordered table-responsive'></table>").appendTo("#ErrorResultArea");
        //Create table header row
        var rowHeader = $("<tr class='info'></tr>").appendTo(table);
        $("<th></th>").text("Error Message").appendTo(rowHeader);
        $("<th></th").text("Error Description ").appendTo(rowHeader);
        $.each(data, function (i, value) {
            //Create new row for each record
            var row = $("<tr class='text-danger'></tr>").appendTo(table);
            $("<td></td>").text(value.ErrorMessage).appendTo(row);
            $("<td></td>").text(value.ErrorDescription).appendTo(row);
        });
        adjustmodal();
    };
    var buildsuccesstable = function (data) {
        //Crate table html tag
        $("<button class='btn btn-primary active pull-right'>Students Added <span class='badge label-primary pull-right'> " + data.length + " </span></button>").appendTo("#SuccessResultArea");
        var table = $("<table id=successtable  class='table table-hour table-bordered table-responsive'></table>").appendTo("#SuccessResultArea");
        //Create table header row
        var rowHeader = $("<tr class='info'></tr>").appendTo(table);
        $("<th></th>").text("RollNo").appendTo(rowHeader);
        $("<th></th").text("Name").appendTo(rowHeader);
        $("<th></th").text("MobileNo").appendTo(rowHeader);
        $.each(data, function (i, value) {
            //Create new row for each record
            var row = $("<tr class='text-success'></tr>").appendTo(table);
            $("<td></td>").text(value.RollNo).appendTo(row);
            $("<td></td>").text(value.Name).appendTo(row);
            $("<td></td>").text(value.Mobile).appendTo(row);
        });
        adjustmodal();


    };
});