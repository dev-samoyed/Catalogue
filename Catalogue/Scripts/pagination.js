function pagination() {
    $(document).ready(function () {
        $("#results_2").hide();
        $(document).on("click", "#contentPager a[href]", post_request);
        $("#position-select-list").on("change", post_request_without_page);
        $("#department-select-list").on("change", post_request_without_page);
        $("#administration-select-list").on("change", post_request_without_page);
        $("#division-select-list").on("change", post_request_without_page);
    });
}

function post_request_without_page() {

    $("#results_1").hide();

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
            var results_1 = $("#results_1");
            var results_2 = $("#results_2");

            if (results_1.is("visible")) {
                results_1.hide();

                results_2.show();
                results_2.html(result);
            } else {
                results_2.hide();

                results_1.show();
                results_1.html(result);
            }
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
            $("#results_1").show();
            $("#results_1").html(result);
        }
    });
    return false;
}