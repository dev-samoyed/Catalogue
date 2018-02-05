﻿function pagination() {
    $(document).ready(function () {
        $(document).on("click", "#contentPager a[href]", post_request);
        $("#position-select-list").on("change", post_request_without_page);
        $("#department-select-list").on("change", post_request_without_page);
        $("#administration-select-list").on("change", post_request_without_page);
        $("#division-select-list").on("change", post_request_without_page);
    });
}

function post_request_without_page() {

    $("#result").hide();

    var name = $("#name").val();

    var positionId = $("#position-select-list").val();
    var departmentId = $("#department-select-list").val();
    var administrationId = $("#administration-select-list").val();
    var divisionId = $("#division-select-list").val();

    $.ajax({
        url: "/crud/Search/EmployeeFilter",
        type: 'POST',
        data: {
            "name": name,
            "positionId": positionId,
            "departmentId": departmentId,
            "administrationId": administrationId,
            "divisionId": divisionId
        },
        cache: false,
        success: function (result) {
            $("#results").html(result);
            $("#results").show();
        }
    });
    return false;
}

function post_request() {

    $("#result").hide();

    var name = $("#name").val();

    var positionId = $("#position-select-list").val();
    var departmentId = $("#department-select-list").val();
    var administrationId = $("#administration-select-list").val();
    var divisionId = $("#division-select-list").val();

    var parts = $(this).attr("href").split("?");
    var url = parts[0];
    var page = parts[1].split("=")[1];

    $.ajax({
        url: url,
        type: 'POST',
        data: {
            "name": name,
            "page": page,
            "positionId": positionId,
            "departmentId": departmentId,
            "administrationId": administrationId,
            "divisionId": divisionId
        },
        cache: false,
        success: function (result) {
            $("#results").html(result);
            $("#results").show();
        }
    });
    return false;
}