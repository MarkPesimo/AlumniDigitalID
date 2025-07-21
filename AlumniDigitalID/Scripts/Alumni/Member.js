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

        //new DataTable('#members-table');

        //$('.dt-length').addClass('d-none');


        new DataTable('#members-table', {
            searching: false
        });

        $('.dt-length').addClass('d-none');

        //  Custom search functionality ---- kbejar  7/3/25 ----
        $('#member-search').on('keyup', function () {
            const value = $(this).val().toLowerCase();
            $('#members-table tbody tr').filter(function () {
                $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
            });
        });
    });

    //===============================================BEGIN EDIT==================================================================================
    $('#members-table').on('click', '.edit-member-btn', function () {
        var _guid = $(this).attr("guid");

        ShowLoading('SHOW');
        $.ajax({
            type: "GET",
            url: '/Alumni/Edit',
            data: { '_guid': _guid },
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (response) {
                $('#edit_member_modal').find(".modal-body").innerHTML = '';
                $('#edit_member_modal').find(".modal-body").html(response);
                $("#edit_member_modal").modal('show');
                ShowLoading('HIDE');
            },
            failure: function (response) { LogError(response); },
            error: function (response) { LogError(response); }
        });
    });

    $('#edit_member_modal').on('click', '#edit-member-button', function (e) {
        ShowLoading('SHOW');
        $.ajax({
            url: '/Alumni/Edit',
            type: "POST",
            data: $('#edit-member-form').serialize(),
            dataType: 'json',
            success: function (result) {
                if (result.Result == "ERROR") { ValidationError(result); }
                else {
                    ShowSuccessMessage('Member information successfully updated.')
                    $("#edit_member_modal").modal('hide');
                    ShowLoading("HIDE");
                }
            },
            failure: function (response) { LogError(response); },
            error: function (response) { LogError(response); }
        });
    });
    //===============================================END EDIT==================================================================================

    //===============================================BEGIN ACTIVATE==================================================================================
    $('#members-table').on('click', '.activate-member-btn', function () {
        var _guid = $(this).attr("guid");

        ShowLoading('SHOW');
        $.ajax({
            type: "GET",
            url: '/Alumni/_Activate',
            data: { '_guid': _guid },
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (response) {
                $('#activate_member_modal').find(".modal-body").innerHTML = '';
                $('#activate_member_modal').find(".modal-body").html(response);
                $("#activate_member_modal").modal('show');
                ShowLoading('HIDE');
            },
            failure: function (response) { LogError(response); },
            error: function (response) { LogError(response); }
        });
    });

    $('#activate_member_modal').on('click', '#activate-member-button', function (e) {
        ShowLoading('SHOW');
        $.ajax({
            url: '/Alumni/ActivateDeactivate',
            type: "POST",
            data: $('#activate-member-form').serialize(),
            dataType: 'json',
            success: function (result) {
                if (result.Result == "ERROR") { ValidationError(result); }
                else { window.location.href = "/Alumni/Members"; }
            },
            failure: function (response) { LogError(response); },
            error: function (response) { LogError(response); }
        });
    });
    //===============================================END ACTIVATE==================================================================================


    //===============================================BEGIN DEACTIVATE==================================================================================
    $('#members-table').on('click', '.deactivate-member-btn', function () {
        var _guid = $(this).attr("guid");

        ShowLoading('SHOW');
        $.ajax({
            type: "GET",
            url: '/Alumni/_Deactivate',
            data: { '_guid': _guid },
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (response) {
                $('#deactivate_member_modal').find(".modal-body").innerHTML = '';
                $('#deactivate_member_modal').find(".modal-body").html(response);
                $("#deactivate_member_modal").modal('show');
                ShowLoading('HIDE');
            },
            failure: function (response) { LogError(response); },
            error: function (response) { LogError(response); }
        });
    });

    $('#deactivate_member_modal').on('click', '#deactivate-member-button', function (e) {
        ShowLoading('SHOW');
        $.ajax({
            url: '/Alumni/ActivateDeactivate',
            type: "POST",
            data: $('#deactivate-member-form').serialize(),
            dataType: 'json',
            success: function (result) {
                if (result.Result == "ERROR") { ValidationError(result); }
                else { window.location.href = "/Alumni/Members"; }
            },
            failure: function (response) { LogError(response); },
            error: function (response) { LogError(response); }
        });
    });
    //===============================================END DEACTIVATE==================================================================================

    //===============================================BEGIN ADD==================================================================================
    $("#add-member-btn").click(function (e) {
        ShowLoading('SHOW');
        $.ajax({
            type: "GET",
            url: '/Alumni/_Add',
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (response) {

                $('#add_member_modal').find(".modal-body").innerHTML = '';
                $('#add_member_modal').find(".modal-body").html(response);
                $("#add_member_modal").modal('show');
                ShowLoading('HIDE');
            },
            failure: function (response) { LogError(response); },
            error: function (response) { LogError(response); }
        });
    });

    $('#add_member_modal').on('click', '#add-members-button', function (e) {
        //$file = $("#add_perks_Attachment");
        //var $filepath = $.trim($file.val());
        //if ($filepath == "") {
        //    ShowWarningMessage('Please select a file, Attachment is required.')
        //    return;
        //}


        ShowLoading('SHOW');
        $.ajax({
            url: '/Alumni/_Add',
            type: "POST",
            data: $('#add-member-form').serialize(),
            dataType: 'json',
            success: function (result) {
                if (result.Result == "ERROR") { ValidationError(result); }
                else {
                    $("#add_member_modal").modal('hide');
                     
                    ShowSuccessMessage('Alumni member successfully created.');

                    //window.location.href = "/Perks/PerksIndex";
                }
            },
            failure: function (response) { LogError(response); },
            error: function (response) { LogError(response); }
        });
    });
    //===============================================END ADD==================================================================================


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