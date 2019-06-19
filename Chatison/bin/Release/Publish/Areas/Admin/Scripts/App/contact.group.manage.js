const contactGroupManageVm = new function () {
    const self = this;
    const table = $("#grid-groups");
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
                url: "/contactGroup/manage",
                method: "POST",
                data: function (d) {
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
                                <input type="checkbox" value="" class="m-checkable"  id="checkBody"/>
                                <span></span>
                            </label>`;
                    }
                },
                {
                    data: "name",
                    title: "name",
                    width: "20%",
                    render: function (data, type, row) {
                        return `<a href='/admin/contact/manage/${row.id}'>${data}</a>`;
                    }
                },
                {
                    data: "createdAt",
                    title: "Created",
                    width: "10%",
                    render: function (data) {
                        return moment(data).format(common.dateFormat);
                    }
                },
                {
                    data: "totalContacts",
                    title: "Contacts",
                    width: "20%"
                },
                {
                    data: "totalInvalid",
                    title: "Invalid",
                    width: "20%"
                },
                {
                    data: "totalOptOuts",
                    title: "Opt-outs",
                    width: "20%"
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
                                    <span class="dropdown-item cursor-pointer" onclick="contactGroupManageVm.exportContacts(${data.id})"><i class="la la-cloud-upload"></i> Export Contacts</span>
                                    <span class="dropdown-item cursor-pointer" onclick="contactGroupManageVm.openEditGroupDialog(${data.id},'${data.name}')"><i class="la la-edit"></i> Edit Group</span>
                                    <span class="dropdown-item cursor-pointer" onclick="contactGroupManageVm.deleteGroup(${data.id})"><i class="la la-trash"></i> Delete</span>
                                </div>
                            </span>`;
                    }
                }
            ],
            fnDrawCallback: function () {
                if ($(".pagination li").length <= 3)
                    $(".pagination").hide();

                setTimeout(() => {
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
            console.log("SDFdsgfdsgs");
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
        filterKey = $("#contactGroupSearch").val();
        $("#grid-groups").DataTable().ajax.reload();
    };

    self.openAddGroupDialog = function () {
        self.resetAddGroupForm();
        $("#add-group-modal").modal("show");
    };

    self.closeAddGroupDialog = function () {
        self.resetAddGroupForm();
        $("#add-group-modal").modal("hide");
    };

    self.resetAddGroupForm = function () {
        $("#FrmAddGroup")[0].reset();
        common.clearFormFields("#FrmAddGroup");
    };

    self.addGroup = function (e) {
        e.preventDefault();

        if ($("#FrmAddGroup").valid() === false) {
            return;
        }

        const model = common.getFormJson("#FrmAddGroup");
        const actionUrl = $("#FrmAddGroup").attr("action");

        common.showLoader("#add-group-modal .modal-dialog");
        $.post(actionUrl,
            model,
            function (res) {
                common.hideLoader("#add-group-modal .modal-dialog");
                if (res.status === "error") {
                    common.processErrorResponse(res);
                    return;
                }
                table.DataTable().ajax.reload();
                alertify.success("Contact group has been added successfully.");
                self.closeAddGroupDialog();
            });
    };

    self.openEditGroupDialog = function (id, name) {
        self.resetEditGroupForm();

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
                table.DataTable().ajax.reload();
                alertify.success("Contact group has been updated successfully.");
                self.closeEditGroupDialog();
            });
    };

    self.deleteGroup = function (id) {
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
                        table.DataTable().ajax.reload();
                        alertify.success("Contact group has been deleted successfully.");
                    });
            },
            null);
    };

    self.exportContacts = function (id) {
        location.href = `/admin/contact/export?groupId=${id}`;
    };

    /****** start: add contacts  *******/

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

    self.getGroupDropdown = function () {
        const elemId = $("#FrmAddContact #containers-group select").length;

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
        $("#containers-group").append(self.getGroupDropdown());
    };

    self.removeContactGroupDropdown = function (formId) {
        const totalItems = $(`#${formId} #containers-group select`).length;

        if (totalItems <= 1) {
            alertify.error("You can not delete all groups");
            return;
        }

        $(`#${formId} #containers-group select:last`).remove();
    };

    /****** end: add contacts  *******/
};

$(document).ready(function () {
    const frmAddContactOptions = {
        clearForm: true,
        clearFields: true,
        resetForm: true,
        beforeSubmit: function () {
            common.showLoader("#add-contact-modal .modal-dialog");
        },
        success: function (res, statusText, xhr) {
            common.hideLoader("#add-contact-modal .modal-dialog");
            if (res.status === "error") {
                common.processErrorResponse(res);
                return;
            }
            contactGroupManageVm.closeAddContactDialog();
            $("#grid-groups").DataTable().ajax.reload();
            alertify.success("Contact has been added successfully.");
        }
    };

    $("#FrmAddContact").ajaxForm(frmAddContactOptions);

    contactGroupManageVm.bindDataTable();

    $("#contactGroupSearch").on("keyup",
        function () {
            contactGroupManageVm.doFilter();
        });

    contactGroupManageVm.addContactGroupDropdown("FrmAddContact");
});