var dismiss = function (id) {
    $.ajax({
        method: "POST",
        url: "/Authorized/DismissNotification/" + id,
        dataType: "json",
        success: function (data) {
            console.log(data);
            notifications();
        }
    });
}

var approve = function (id) {
    $.ajax({
        method: "POST",
        url: "/Authorized/ApproveNotification/" + id,
        dataType: "json",
        success: function (data) {
            console.log(data);
            notifications();
        }
    });
}

var notifications = function (first = false) {
    $.ajax({
        method: "POST",
        url: "/Authorized/Notifications",
        dataType: "json",
        success: function (data) {
            if (!first) {
                $("#navbarDropdown > .notificationCounter").remove();
                $("#notificationDropdown > .notification").remove();
            }

            $("#navbarDropdown").append("<span class='badge badge-light notificationCounter'>" + data.length + "</span>");
            data.forEach(function (m) {
                $("#notificationDropdown").append("<div class='dropdown-item notification container'>" + m.message + "<button onclick='approve(" + m.id + ")' class='btn btn-primary btn-sm float-right'>Apstiprināt</button><button onclick='dismiss(" + m.id + ")' class='btn btn-danger btn-sm float-right'>X</button></div>");
            });
        }
    });
};

$(notifications(true));