var rowNumber = 0;
var tabIndex = 0;
var sColumnNr;

Handlebars.registerHelper('rowNumber', function (obj) {
    return ++rowNumber;
});

Handlebars.registerHelper('curRowNumber', function (obj) {
    return rowNumber;
});

Handlebars.registerHelper("tabIndex", function (columnNr, obj) {
    if (sColumnNr != columnNr) {
        sColumnNr = columnNr;
        tabIndex++;
    }
    return tabIndex;
});

Handlebars.registerHelper("formatDate", function (datetime) {
    if (!datetime || typeof datetime === "undefined")
        return "";
    if (moment) {
        var x = moment(datetime).format('DD-MM-YYYY');
        return x;
    }
    else {
        return datetime;
    }
});

Handlebars.registerHelper("formatMoney", function (money) {
    var num = Number(money);
    if (isNaN(num)) {
        return money;
    }
    // If there is no decimal, or the decimal is less than 2 digits, toFixed
    if (String(num).split(".").length < 2 || String(num).split(".")[1].length <= 2) {
        num = num.toFixed(2);
    }
    return num.toString().replace(".", ",").replace(/\B(?=(\d{3})+(?!\d))/g, ".");
});

Handlebars.registerHelper('ifEmpty', function (v1, options) {
    if (v1 == undefined || v1 == "") {
        return options.fn(this);
    }
    return options.inverse(this);
});

Handlebars.registerHelper('ifNotEmpty', function (v1, options) {
    if (v1 != undefined && v1 != "") {
        return options.fn(this);
    }
    return options.inverse(this);
});

Handlebars.registerHelper('ifOrNot', function (v1, v2, options) {
    if (v1 || !v2) {
        return options.fn(this);
    }
    return options.inverse(this);
});

Handlebars.registerHelper('ifAndNot', function (v1, v2, options) {
    if (v1 && !v2) {
        return options.fn(this);
    }
    return options.inverse(this);
});

Handlebars.registerHelper('or', function (v1, v2, options) {
    if (v1 || v2) {
        return options.fn(this);
    }
    return options.inverse(this);
});

Handlebars.registerHelper('ifEqual', function (v1, v2, options) {
    if (v1 == v2) {
        return options.fn(this);
    }
    return options.inverse(this);
});

Handlebars.registerHelper('ifEqualToInt', function (v1, v2, options) {
    if (v1 == parseInt(v2)) {
        return options.fn(this);
    }
    return options.inverse(this);
});

Handlebars.registerHelper('ifNotEqualToInt', function (v1, v2, options) {
    if (v1 != parseInt(v2)) {
        return options.fn(this);
    }
    return options.inverse(this);
});

Handlebars.registerHelper('ifNotEqual', function (v1, v2, options) {
    if (v1 != v2) {
        return options.fn(this);
    }
    return options.inverse(this);
});

Handlebars.registerHelper('ifContains', function (v1, v2, options) {
    if (v1.indexOf(v2) > -1) {
        return options.fn(this);
    }
    return options.inverse(this);
});

Handlebars.registerHelper('forceCurrencyToTwoDecimals', function (currency) {
    return roundCurrencyToTwoDecimals(currency);
});

Handlebars.registerHelper('forceCurrencyToTwoDecimalsExceptEmpty', function (currency) {
    return currency != null ? roundCurrencyToTwoDecimals(currency) : "";
});