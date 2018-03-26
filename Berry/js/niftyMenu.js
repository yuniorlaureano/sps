$(document).ready(function () {
    $("#mainnav-menu").find("li").children("a").on('click', function () {

        activeLink(this.innerText);

    });

    var activeLink = function (name) {

        $.cookie('settings-nav-active')

        $("#mainnav-menu").find("li:contains('" + name + "')").parent("ul").collapse('show');

        if (($("#mainnav-menu").find("li a:contains('" + name + "')").parent("li").children("ul")).length == 0) {

            $("#mainnav-menu").find("li a:contains('" + name + "')").parent("li").addClass("active-link");
        }

        setCookie("settings-nav-active", name);
    }


    //Nav item active
    if ($.cookie('settings-nav-active')) {
        activeLink($.cookie('settings-nav-active'));
    }
});