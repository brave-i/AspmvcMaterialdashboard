const contactManageVm = new function () {
    const self = this;
    const table = $("#grid-contacts");
    const groups = JSON.parse($("#hidGroups").val());

    let filterKey = null;

    self.bindDataTable = function () {
        const grid = table.DataTable({
            dom: `<'row'<'col-sm-12'tr>><'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7 dataTables_pager'lp>>`,
            processing: true,
            serverSide: true,
            language: {
                loadingRecords: "&nbsp;",
                processing: "<img src='/areas/admin/content/images/ajax-loader-m.gif'/>",
                searchPlaceholder: "Filter records",
                paginate: {
                    first: "First",
                    last: "Last",
                    next: "Next",
                    previous: "Prev"
                }
            },
            order: [[1, 'asc']],
            paging: true,
            pageLength: 10,
            lengthMenu: [10, 15, 25, 50, 100],
            ajax: {
                url: "/contact/manage",
                method: "POST",
                data: function (d) {
                    d.groupId = $("#hidGroupId").val();
                    d.search = { value: filterKey };
                    return d;
                }
            },
            headerCallback: function (thead, data, start, end, display) {
                thead.getElementsByTagName('th')[0].innerHTML = `
                        <label class="kt-checkbox kt-checkbox--single kt-checkbox--solid">
                            <input type="checkbox" value="" class="m-group-checkable" id="checkHeader"/>
                            <span></span>
                        </label>`;
            },
            columns: [
                {
                    data: null,
                    title: "",
                    width: '30px',
                    className: 'dt-right',
                    sortable: false,
                    render: function (data, type, full, meta) {
                        return `
                            <label class="kt-checkbox kt-checkbox--single kt-checkbox--solid">
                                <input type="checkbox" value="" class="m-checkable" id="checkBody"/>
                                <span></span>
                            </label>`;
                    }
                },
                {
                    data: "firstName",
                    title: "Name",
                    width: "30%",
                    render: function (data, type, row) {
                        const optedOutHtml = "<i class='fa fa-frown fs-18 m-l-5 text-warning' data-skin='dark' data-toggle='kt-tooltip' data-placement='top' title='' data-original-title='Opted-Out'></i>";
                        return `${row.firstName} ${row.lastName} ${row.isOptout ? optedOutHtml : ""}`;
                    }
                },
                {
                    data: "email",
                    title: "Email",
                    width: "20%"
                },
                {
                    data: "phone",
                    title: "Phone",
                    width: "15%",
                    render: function (data) {
                        return common.formatPhone(data);
                    }
                },
                {
                    data: "createdAt",
                    title: "Added",
                    width: "10%",
                    render: function (data) {
                        return moment(data).format(common.dateFormat);
                    }
                },
                {
                    data: "source",
                    title: "Source",
                    width: "15%",
                    render: function (data) {
                        return data === 0 ? "Manually Added" : "Imported";
                    }
                },
                {
                    data: null,
                    title: "Actions",
                    className: "text-center",
                    sortable: false,
                    width: "10%",
                    render: function (data) {
                        return `
                            <span class="dropdown">
                                <a href="#" class="btn btn-sm btn-clean btn-icon btn-icon-md" data-toggle="dropdown" aria-expanded="true">
                                    <i class="la la-ellipsis-h"></i>
                                </a>
                                <div class="dropdown-menu dropdown-menu-right">
                                    <span class="dropdown-item cursor-pointer"><i class="la la-comment"></i> Send a message</span>
                                    <span class="dropdown-item cursor-pointer" onclick="contactManageVm.openEditContactDialog('${data.id}')"><i class="la la-edit"></i> Edit Contact</span>
                                    <span class="dropdown-item cursor-pointer" onclick="contactManageVm.deleteContact('${data.id}')"><i class="la la-trash"></i> Delete</span>
                                </div>
                            </span>`;
                    }
                }
            ],
            fnDrawCallback: function () {
                if ($(".pagination li").length <= 3)
                    $(".pagination").hide();

                setTimeout(() => {
                    $('[data-toggle="kt-tooltip"]').each(function () {
                        common.initTooltip($(this));
                    });
                    $(".table-checkable thead #checkHeader").on("change",
                        function () {
                            const checked = $(this).prop("checked");
                            $(".table-checkable tbody #checkBody").each(function () {
                                $(this).prop("checked", checked);
                            });

                        });
                }, 500);
            }
        });


        table.on('change', '.kt-group-checkable', function () {
            var set = $(this).closest('table').find('td:first-child .kt-checkable');
            var checked = $(this).is(':checked');

            $(set).each(function () {
                if (checked) {
                    $(this).prop('checked', true);
                    $(this).closest('tr').addClass('active');
                }
                else {
                    $(this).prop('checked', false);
                    $(this).closest('tr').removeClass('active');
                }
            });
        });

        table.on('change', 'tbody tr .kt-checkbox', function () {
            $(this).parents('tr').toggleClass('active');
        });
    };

    self.doFilter = function () {
        filterKey = $("#contactSearch").val();
        $("#grid-contacts").DataTable().ajax.reload();
    };

    self.getGroupDropdown = function (formId) {
        const elemId = $(`#${formId} #containers-group select`).length;

        let options = "<option value=''>Select Group</option>";

        groups.map((item) => {
            options += `<option value="${item.Id}">${item.Name}</option>`;
        });

        return `<select class="form-control m-b-15" name="GroupIds[${elemId}]" id="GroupId_${elemId}">${options}</select>`;
    };

    self.addContactGroupDropdown = function (formId) {
        if (groups.length === 0) {
            return;
        }
        if (($(`#${formId} #containers-group select`).length) >= groups.length) {
            alertify.error("You can add maximum " + groups.length + " groups only");
            return;
        }
        $(`#${formId} #containers-group`).append(self.getGroupDropdown(formId));
    };

    self.removeContactGroupDropdown = function (formId) {
        const totalItems = $(`#${formId} #containers-group select`).length;

        if (totalItems <= 1) {
            alertify.error("You can not delete all groups");
            return;
        }

        $(`#${formId} #containers-group select:last`).remove();
    };

    /****** start: add contact  *******/

    self.resetAddContactForm = function () {
        $("#FrmAddContact").resetForm();
        $("#FrmAddContact").clearForm();
    };

    self.openAddContactDialog = function () {
        self.resetAddContactForm();

        $("#FrmAddContact select").each(function () {
            $(this).val("");
        });

        $("#add-contact-modal").modal("show");
    };

    self.closeAddContactDialog = function () {
        self.resetAddContactForm();
        $("#add-contact-modal").modal("hide");
    };

    /****** end: add contact  *******/

    /****** start: edit contact  *******/

    self.resetEditContactForm = function () {
        $("#FrmEditContact").resetForm();
        $("#FrmEditContact").clearForm();
    };

    self.openEditContactDialog = function (id) {
        self.resetEditContactForm();

        $("#FrmEditContact select").each(function () {
            $(this).val("");
        });
        $("#edit-contact-modal").modal("show");
        $("#edit-contact-modal").on("shown.bs.modal", function () {
            self.getContactForEdit(id);
        });
    };

    self.closeEditContactDialog = function () {
        self.resetEditContactForm();
        $("#edit-contact-modal").modal("hide");
    };

    self.getContactForEdit = function (id) {
        common.showLoader("#edit-contact-modal .modal-dialog");
        $.get(`/admin/contact/edit/${id}`,
            function (res) {
                common.hideLoader("#edit-contact-modal .modal-dialog");
                self.bindEditForm(res);
            });
    };

    self.bindEditForm = function (model) {
        $("#FrmEditContact #Id").val(model.id);
        $("#FrmEditContact #FirstName").val(model.firstName);
        $("#FrmEditContact #LastName").val(model.lastName);
        $("#FrmEditContact #Phone").val(model.phone);
        $("#FrmEditContact #Email").val(model.email);
        $("#FrmEditContact #ProviderId").val(model.providerId);
        if (model.isOptOut) {
            $("#FrmEditContact #IsOptOut").prop("checked", true);
            $("#FrmEditContact #IsOptOut").next().val(true);
        }
        if (model.isAdmin) {
            $("#FrmEditContact #IsAdmin").prop("checked", true);
            $("#FrmEditContact #IsAdmin").next().val(true);
        }

        $("#FrmEditContact #containers-group select").each(function () {
            $(this).remove();
        });

        if (model.groupIds && model.groupIds.length !== 0) {
            let counter = 0;
            model.groupIds.map((item) => {
                self.addContactGroupDropdown("FrmEditContact");
                $(`#FrmEditContact #containers-group #GroupId_${(counter++)}`).val(item);
            });
        } else {
            self.addContactGroupDropdown("FrmEditContact");
        }

        $("#FrmEditContact #Phone").mask("(999) 999-9999");
    };

    /****** end: add contact  *******/

    self.deleteContact = function (id) {
        alertify.confirm("Are you sure you want to delete the selected contact?",
            function () {
                common.showLoader(".kt-portlet__body");
                $.post("/admin/contact/delete",
                    { id },
                    function (res) {
                        common.hideLoader(".kt-portlet__body");
                        if (res.status === "error") {
                            common.processErrorResponse(res);
                            return;
                        }
                        table.DataTable().ajax.reload();
                        alertify.success("Contact has been deleted successfully.");
                    });
            },
            null);
    };

    self.exportContacts = function () {
        location.href = `/admin/contact/export?groupId=${$("#hidGroupId").val()}`;
    };

    /****** start: manage group  *******/

    self.deleteGroup = function () {
        var id = $("#hidGroupId").val();
        alertify.confirm("Are you sure you want to delete the selected group?",
            function () {
                common.showLoader(".kt-portlet__body");
                $.post("/admin/contactGroup/delete",
                    { id },
                    function (res) {
                        common.hideLoader(".kt-portlet__body");
                        if (res.status === "error") {
                            common.processErrorResponse(res);
                            return;
                        }
                        alertify.success("Contact group has been deleted successfully.");
                        setTimeout(() => {
                            location.href = "/admin/contactGroup/manage";
                        },
                            1000);
                    });
            },
            null);
    };

    self.openEditGroupDialog = function () {
        self.resetEditGroupForm();

        const id = $("#hidGroupId").val();
        const name = $("#hidGroupName").val();

        $("#edit-group-modal #Id").val(id);
        $("#edit-group-modal #Name").val(name);

        $("#edit-group-modal").modal("show");
    };

    self.closeEditGroupDialog = function () {
        self.resetEditGroupForm();
        $("#edit-group-modal").modal("hide");
    };

    self.resetEditGroupForm = function () {
        $("#FrmEditGroup")[0].reset();
        common.clearFormFields("#FrmEditGroup");
    };

    self.editGroup = function (e) {
        e.preventDefault();

        if ($("#FrmEditGroup").valid() === false) {
            return;
        }

        const model = common.getFormJson("#FrmEditGroup");
        const actionUrl = $("#FrmEditGroup").attr("action");

        common.showLoader("#edit-group-modal .modal-dialog");
        $.post(actionUrl,
            model,
            function (res) {
                common.hideLoader("#edit-group-modal .modal-dialog");
                if (res.status === "error") {
                    common.processErrorResponse(res);
                    return;
                }
                alertify.success("Contact group has been updated successfully.");
                self.closeEditGroupDialog();
                setTimeout(() => { location.reload(); }, 500);
            });
    };
    /****** end: manage group  *******/
};

$(document).ready(function () {

    contactManageVm.bindDataTable();

    /****  start: add contact **/

    const frmAddContactOptions = {
        clearForm: false,
        clearFields: false,
        resetForm: false,
        beforeSubmit: function () {
            common.showLoader("#add-contact-modal .modal-dialog");
        },
        success: function (res, statusText, xhr) {
            common.hideLoader("#add-contact-modal .modal-dialog");
            if (res.status === "error") {
                common.processErrorResponse(res);
                return;
            }
            contactManageVm.closeAddContactDialog();
            $("#grid-contacts").DataTable().ajax.reload();
            alertify.success("Contact has been added successfully.");
        }
    };

    $("#FrmAddContact").ajaxForm(frmAddContactOptions);

    contactManageVm.addContactGroupDropdown("FrmAddContact");

    /****  end: add contact **/

    /****  start: edit contact **/

    const frmEditContactOptions = {
        clearForm: false,
        clearFields: false,
        resetForm: false,
        beforeSubmit: function () {
            common.showLoader("#edit-contact-modal .modal-dialog");
        },
        success: function (res, statusText, xhr) {
            common.hideLoader("#edit-contact-modal .modal-dialog");
            if (res.status === "error") {
                common.processErrorResponse(res);
                return;
            }
            contactManageVm.closeEditContactDialog();
            $("#grid-contacts").DataTable().ajax.reload();
            alertify.success("Contact has been updated successfully.");
        }
    };

    $("#FrmEditContact").ajaxForm(frmEditContactOptions);

    /****  end: edit contact **/

    $("#contactSearch").on("keyup",
        function () {
            contactManageVm.doFilter();
        });


});