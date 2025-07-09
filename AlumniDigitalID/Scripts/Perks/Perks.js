$(function () {
    $(document).ready(function () {
        //new DataTable('#perks-table', {
        //    layout: {
        //        topStart: {
        //            buttons: [
        //                {
        //                    text: 'Add new button',
        //                    action: function (e, dt, node, config) {
        //                        dt.button().add(1, {
        //                            text: 'Button ' + counter++,
        //                            action: function () {
        //                                //this.remove();
        //                                alert('clicked');
        //                            }
        //                        });
        //                    }
        //                }
        //            ]
        //        }
        //    }
        //});

        new DataTable('#perks-table', {
            searching: false
        });

        $('.dt-length').addClass('d-none');

        //  Custom search functionality ---- kbejar  7/3/25 ----
        $('#perk-search').on('keyup', function () {
            const value = $(this).val().toLowerCase();
            $('#perks-table tbody tr').filter(function () {
                $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
            });
        });
    });

    //$('.perks-control').on('click', '.perk-btn-primary', function (e) {
    //    e.preventDefault();

    //    var Id = $(this).attr("id");
    //    alert(Id);
    //});

    //===============================================BEGIN EDIT==================================================================================
    $('#perks-table').on('click', '.edit-perk-btn', function () {
        var _guid = $(this).attr("guid");

        ShowLoading('SHOW');
        $.ajax({
            type: "GET",
            url: '/Perks/_Edit',
            data: { '_guid': _guid },
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (response) {                
                $('#edit_perks_modal').find(".modal-body").innerHTML = '';
                $('#edit_perks_modal').find(".modal-body").html(response);
                $("#edit_perks_modal").modal('show');
                ShowLoading('HIDE');                
            },
            failure: function (response) { LogError(response); },
            error: function (response) { LogError(response); }
        });
    });

    $('#edit_perks_modal').on('click', '#update-perks-button', function (e) {
        ShowLoading('SHOW');
        $.ajax({
            url: '/Perks/Manage',
            type: "POST",
            data: $('#edit-perks-form').serialize(),
            dataType: 'json',
            success: function (result) {
                if (result.Result == "ERROR") { ValidationError(result); }
                else {
                    $("#edit_perks_modal").modal('hide');

                    $file = $("#edit_perks_Attachment");
                    var $filepath = $.trim($file.val());

                    if ($filepath != "") {
                        PerksAttachment(result.Id, '#edit_perks_Attachment', 'Perks successfully updated.')                        
                    }

                    

                    //window.location.href = "/Perks/PerksIndex";
                }
            },
            failure: function (response) { LogError(response); },
            error: function (response) { LogError(response); }
        });
    });
    //===============================================END EDIT==================================================================================

    //===============================================BEGIN ADD==================================================================================
    $("#add-perk-btn").click(function (e) {
        ShowLoading('SHOW');
        $.ajax({
            type: "GET",
            url: '/Perks/_Add',
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (response) {
                
                $('#add_perks_modal').find(".modal-body").innerHTML = '';
                $('#add_perks_modal').find(".modal-body").html(response);
                $("#add_perks_modal").modal('show');
                ShowLoading('HIDE');
            },
            failure: function (response) { LogError(response); },
            error: function (response) { LogError(response); }
        });
    });

    $('#add_perks_modal').on('click', '#add-perks-button', function (e) {
        $file = $("#add_perks_Attachment");
        var $filepath = $.trim($file.val());
        if ($filepath == "") {
            ShowWarningMessage('Please select a file, Attachment is required.')
            return;
        }


        ShowLoading('SHOW');
        $.ajax({
            url: '/Perks/Manage',
            type: "POST",
            data: $('#add-perks-form').serialize(),
            dataType: 'json',
            success: function (result) {
                if (result.Result == "ERROR") { ValidationError(result); }
                else {
                    $("#add_perks_modal").modal('hide');

          
                    PerksAttachment(result.Id, '#add_perks_Attachment', 'Perks successfully created.')
                  
                    
                    //window.location.href = "/Perks/PerksIndex";
                }
            },
            failure: function (response) { LogError(response); },
            error: function (response) { LogError(response); }
        });
    });
    //===============================================END ADD==================================================================================

    //===============================================BEGIN ACTIVATE==================================================================================
    $('#perks-table').on('click', '.activate-perk-btn', function () {
        var _guid = $(this).attr("guid");

        ShowLoading('SHOW');
        $.ajax({
            type: "GET",
            url: '/Perks/_Activate',
            data: { '_guid': _guid },
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (response) {
                $('#activate_perks_modal').find(".modal-body").innerHTML = '';
                $('#activate_perks_modal').find(".modal-body").html(response);
                $("#activate_perks_modal").modal('show');
                ShowLoading('HIDE');
            },
            failure: function (response) { LogError(response); },
            error: function (response) { LogError(response); }
        });
    });

    $('#activate_perks_modal').on('click', '#activate-perks-button', function (e) {
        ShowLoading('SHOW');
        $.ajax({
            url: '/Perks/Manage',
            type: "POST",
            data: $('#activate-perks-form').serialize(),
            dataType: 'json',
            success: function (result) {
                if (result.Result == "ERROR") { ValidationError(result); }
                else { window.location.href = "/Perks/PerksIndex"; }
            },
            failure: function (response) { LogError(response); },
            error: function (response) { LogError(response); }
        });
    });
    //===============================================END ACTIVATE==================================================================================

    //===============================================BEGIN DEACTIVATE==================================================================================
    $('#perks-table').on('click', '.deactivate-perk-btn', function () {
        var _guid = $(this).attr("guid");

        ShowLoading('SHOW');
        $.ajax({
            type: "GET",
            url: '/Perks/_Deactivate',
            data: { '_guid': _guid },
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (response) {
                $('#deactivate_perks_modal').find(".modal-body").innerHTML = '';
                $('#deactivate_perks_modal').find(".modal-body").html(response);
                $("#deactivate_perks_modal").modal('show');
                ShowLoading('HIDE');
            },
            failure: function (response) { LogError(response); },
            error: function (response) { LogError(response); }
        });
    });

    $('#deactivate_perks_modal').on('click', '#deactivate-perks-button', function (e) {
        ShowLoading('SHOW');
        $.ajax({
            url: '/Perks/Manage',
            type: "POST",
            data: $('#deactivate-perks-form').serialize(),
            dataType: 'json',
            success: function (result) {
                if (result.Result == "ERROR") { ValidationError(result); }
                else { window.location.href = "/Perks/PerksIndex"; }
            },
            failure: function (response) { LogError(response); },
            error: function (response) { LogError(response); }
        });
    });
    //===============================================END DEACTIVATE==================================================================================

    function PerksAttachment(_id, _file, _msg) {
        var formData = new FormData();
        var _Attachement = $(_file)[0].files[0];

        console.log(_file);

        formData.append('_id', _id);
        formData.append('Perks_Attachment', _Attachement);

        $.ajax({
            url: '/Perks/_Attachment',
            type: "POST",
            data: formData,
            processData: false,
            contentType: false,
            success: function (result) {
                if (result == "ERROR") {
                    ShowLoading('HIDE');
                    alert('Error in attaching the ' + $file + ' file!')
                }
                else {
                    //window.location.href = "/Perks/PerksIndex";
                    //ClearTable('#leave-table');
                    //LoadDefault();
                    ShowSuccessMessage(_msg);
                    //ShowLoading('HIDE');
                }
            }
        });

    }


    function ShowLoading(show) {
        var x = document.getElementById("preloader");
        if (show === 'SHOW') { x.style.visibility = ''; }
        else { x.style.visibility = 'hidden'; }
    }

    function ShowSuccessMessage(_msg) {
        ShowLoading('HIDE');
        document.getElementById("toasterSuccess-body").innerHTML = _msg;
        const toaster = document.getElementById("toasterSuccess");
        const toasterFunction = bootstrap.Toast.getOrCreateInstance(toaster);
        toasterFunction.show();
    }

    function ShowWarningMessage(_msg) {
        ShowLoading('HIDE');
        document.getElementById("toasterWarning-body").innerHTML = _msg;
        const toaster = document.getElementById("toasterWarning");
        const toasterFunction = bootstrap.Toast.getOrCreateInstance(toaster);
        toasterFunction.show();
    }

    function ValidationError(result) {
        if (result.ElementName != null) {
            var div_validation = document.querySelector('#div-validation');
            div_validation.style.display = "block";

            document.getElementsByName(result.ElementName)[0].focus();
            document.getElementById("error-message-label").innerHTML = "* " + result.Message;
        }
        else { window.alert(result.Message); }
        ShowLoading('HIDE');
        return;
    }
    function LogError(response) {
        ShowLoading('HIDE');
        console.log(response.responseText);
    }
});