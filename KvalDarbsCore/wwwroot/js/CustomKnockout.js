ko.bindingHandlers.date = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        $(element).datepicker();
        $(element).attr("maxlength", "10");

        var defaultdate = ko.utils.unwrapObservable(allBindingsAccessor()['defaultDate']);
        if (defaultdate != null)
            $(element).datepicker("option", "defaultDate", defaultdate);
    },
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var rawDate = ko.utils.unwrapObservable(valueAccessor());
        if (!rawDate)
            return;
        var parsedDate = new Date(rawDate);
        var formattedDate = $.datepicker.formatDate("dd/mm/yy", parsedDate);

        if ($(element).is('span'))
            $(element).html(formattedDate);
        else
            $(element).val(formattedDate);
    }
};