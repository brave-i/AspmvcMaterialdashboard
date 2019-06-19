var common = new function () {

    var self = this;

    self.dateFormat = "MM/DD/YYYY";

    self.isNullOrEmpty = function (strVal) { return (strVal === typeof ("undefined") || strVal === "" || strVal == null); };

    self.getUrl = function (action, controller) {
        if (self.isNullOrEmpty(area))
            return baseUrl + controller + "/" + action;
        //returning with area
        return baseUrl + area + "/" + controller + "/" + action;
    };

    self.constructResponse = function () {
        var response = responseMessage;
        //clearing response from field
        responseMessage = null;
        //validating response
        if (self.isNullOrEmpty(response)) return;
        //checking for notifier
        if (response.indexOf("|") === -1) {
            alertify.alert(response);
            return;
        }
        var strArray = response.split("|");
        if (strArray.length === 0) return;
        switch (strArray[0].toLowerCase()) {
            case "success":
                alertify.success(strArray[1]);
                break;
            case "error":
                alertify.error(strArray[1]);
                break;
            default:
                alertify.warning(strArray[1]);
                break;
        }
    };

    self.getErrorString = function (errors) {
        if (errors === null || errors.length === 0) return null;
        var tempStr = "";
        $.each(errors, function (index, item) {
            tempStr += item + "<br />";
        });
        return tempStr;
    };

    self.processErrorResponse = function (res) {
        if (res.error) {
            alertify.error(res.error);
        }

        if (res.errors) {
            const error = self.getErrorString(res.errors);
            alertify.error(error);
        }
    };

    self.showLoader = function (element) {
        $(element).block({ message: "<img src='/areas/admin/content/images/ajax-loader-m.gif' alt='Loading..'/>" });
    };

    self.hideLoader = function (element) {
        $(element).unblock();
    };

    self.showBtnLoader = function (element) {
        $(element).html('Please wait..');
        $(element).attr("disabled", "disabled");
    };

    self.hideBtnLoader = function (element, caption) {
        if (self.isNullOrEmpty(caption))
            caption = "Submit";
        $(element).html(caption);
        $(element).removeAttr("disabled");
    };

    self.formatPhone = function (input) {
        return input ? input.replace(/(\d{3})(\d{3})(\d{4})/, "($1) $2-$3") : "";
    };

    self.getDummyImageUrl = function (dimension) {
        return "https://dummyimage.com/" + dimension + "/cccccc/777777&text=No%20Image";
    };

    self.getStatusLabel = function (status) {
        switch (status) {
            case 1:
                return "<span class='label label-primary'>Active</span>";
            case 2:
                return "<span class='label label-danger'>Inactive</span>";
            default:
                return "<span class='label label-default'>Created</span>";
        }
    };

    self.bindDefaults = function () {
        $("input[type=text][data-type=integer]").numeric({ decimal: false });
        $("input[type=text][data-type=number]").numeric({ decimalPlaces: 2 });
        $("input[type=text][data-type=number-1]").numeric({ decimalPlaces: 1 });

        $("input[type=text][data-type=phone]").mask("(999) 999 9999");
        $("input[type=text][data-type=ssn]").mask("999-99-9999");
        $("input[type=text][data-type=zip]").mask("99999");
        //restricting special characters
        $("input[type=text]").on("paste", function () { return false; });
        $("input[type=text]").on("drop", function () { return false; });
        $("input[type=text][filter-type='special-chars']").keyup(function () {
            var value = $(this).val();
            var re = /[`~!#$%^&*()|+\=?;:'",<>\{\}\[\]\\\/]/gi;
            var isSplChar = re.test(value);
            if (isSplChar) {
                $(this).val(value.replace(/[`~!#$%^&*()|+\=?;:'",<>\{\}\[\]\\\/]/gi, ""));
            }
        });
    };

    self.clearFormFields = function (element) {
        $(`${element} input[type='text']`).each(function () {
            $(this).val("");
            $(this).next().find(".field-validation-valid").html("");
        });

        $(`${element} textarea`).each(function () {
            $(this).val("");
            $(this).next().find(".field-validation-valid").html("");
        });

        $(`${element} select`).each(function () {
            $(this).val("");
            $(this).next().find(".field-validation-valid").html("");
        });

        $(`${element} input[type='checkbox']`).each(function () {
            $(this).prop("checked", false);
        });


    };

    self.getFormJson = function (element) {
        var model = {};

        $(`${element} input[type='hidden']`).each(function () {
            model[$(this).attr("name")] = $(this).val();
        });

        $(`${element} input[type='text']`).each(function () {
            model[$(this).attr("name")] = $(this).val();
        });

        $(`${element} textarea`).each(function () {
            model[$(this).attr("name")] = $(this).val();
        });
        return model;
    };

    self.initTooltip = function (el) {
        console.log("dsfdsf");
        const skin = el.data('skin') ? 'tooltip-' + el.data('skin') : '';
        const width = el.data('width') == 'auto' ? 'tooltop-auto-width' : '';
        const triggerValue = el.data('trigger') ? el.data('trigger') : 'hover';
        const placement = el.data('placement') ? el.data('placement') : 'left';

        el.tooltip({
            trigger: triggerValue,
            template: '<div class="tooltip ' +
                skin +
                ' ' +
                width +
                '" role="tooltip">\
                <div class="arrow"></div>\
                <div class="tooltip-inner"></div>\
            </div>'
        });
    };
};

$(document).ready(function () {
    alertify.defaults.glossary.title = "<strong>Chatison</strong>";
    alertify.defaults.theme.ok = "btn btn-primary btn-flat btn-sm";
    alertify.defaults.theme.cancel = "btn btn-default btn-flat btn-sm";
    alertify.defaults.transition = "fade";
    alertify.set("notifier", "position", "top-right");

    common.bindDefaults();
    common.constructResponse();
});

