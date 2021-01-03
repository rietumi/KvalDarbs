ko.bindingHandlers.date = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        $(element).datepicker();
        $(element).datepicker("option", "dateFormat", "dd/mm/yy");
        $(element).attr("maxlength", "10");
        $(element).attr("autocomplete", "off");

        var defaultdate = ko.utils.unwrapObservable(allBindingsAccessor()['defaultDate']);
        if (defaultdate != null)
            $(element).datepicker("option", "defaultDate", defaultdate);
    },
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var rawDate = ko.utils.unwrapObservable(valueAccessor());
        if (!rawDate)
            return;
        var formattedDate = rawDate;

        if (rawDate.indexOf('T') != -1) {
            var parsedDate = new Date(rawDate);

            if (isNaN(parsedDate.getTime()))
                parsedDate = rawDate;

            formattedDate = $.datepicker.formatDate("dd/mm/yy", parsedDate);
        }

        if ($(element).is('span'))
            $(element).html(formattedDate);
        else
            $(element).val(formattedDate);
    }
};

ko.bindingHandlers.uniqueId = {
    init: function (element, valueAccessor) {
        var value = valueAccessor();
        value.id = value.id || ko.bindingHandlers.uniqueId.prefix + (++ko.bindingHandlers.uniqueId.counter);

        element.id = value.id;
    },
    counter: 0,
    prefix: "ko_unique"
}

ko.bindingHandlers.uniqueFor = {
    init: function (element, valueAccessor) {
        var value = valueAccessor();
        value.id = value.id || ko.bindingHandlers.uniqueId.prefix + (++ko.bindingHandlers.uniqueId.counter);

        element.setAttribute("for", value.id);
    }
};