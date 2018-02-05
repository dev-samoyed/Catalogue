function pagination() {
    $(document).on("click", "#contentPager a", function () {
        console.log($(this).attr("href"));
    })
}