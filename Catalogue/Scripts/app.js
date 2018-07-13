$(function () {
    $('[data-toggle="tooltip"]').tooltip()
})
function hideAccordion() {
    $("#accordion").hide();
}

function hideEmployeeList() {
    $("#employee-list").hide();
}

function toPrevMain(from = "") {
    if (from == "list") {
        $("#employee-list").empty();
    } else if (from == "admin") {
        $("#name").val("");
        $("#position-select-list").val("");
        $("#department-select-list").val("");
        $("#administration-select-list").val("");
        $("#division-select-list").val("");
        $("#results").empty();
    } else {
        $("#results").empty();
    }  
    $("#accordion").show();
}

    function toPrevEmployeeList() {
        $("#results").empty();
        $("#employee-list").show();
    }

    function removeListAndPagination() {
        $("#listTable").remove();
        $("#paginationToDelete").remove();
    }

    function checkPhotoPreview(){
        $(document).ready(function () {
            $("#imageBrowes").change(function () {
                var File = this.files;
                if (File && File[0]) {
                    ReadImage(File[0]);
                }
            })
        })

        var ReadImage = function (file) {

            var reader = new FileReader;
            var image = new Image;
            var size = ~~(file.size);
            var type = file.type;

            if (type != "image/jpeg" && type != "image/png" && type != "image/jpg") {
                ClearPreview();
                $('#myModal').modal('show');
                $('#MessageError').text('Необходимо выбрать фотографию с расширением JPeG или PNG!');   
            } 
            else{
                reader.readAsDataURL(file);
                reader.onload = function (_file) {

                    image.src = _file.target.result;
                    image.onload = function () {
          
                        $("#targetImg").attr('src', _file.target.result);
                        $("#imgPreview").show();
                                 
                        if (size > 2000000) {
                            ClearPreview();
                            $('#myModal').modal('show');
                            $('#MessageError').text('Размер изображения не должно превышать 2 МБ!');                      
                        } 
                    }
                }
            }
        }
    }

    function ClearPreview () {
        $("#imageBrowes").val('');
        $("#imgPreview").hide()  
    }

    function removeBlock() {
        $("#toRemove").remove();
    }

    $(function () {
        $('[data-toggle="tooltip"]').tooltip()
    })

    //$(function () {
    //    $(".list").selectBoxIt({
    //        theme: "default",
    //        autoWidth: false
    //    });
    //});

    $(document).ready(function(){
        $('#create-form').on('submit', function(e){
            var form = $(this);
            e.preventDefault();
            var elements = $(".validation-span");
            var validated = true;
            $.each(elements, function(e, v){
                validated = $(v).is(':empty');

                if(!validated){
                    $("#se-pre-con").hide();
                    return false;
                }
            });

            if(!validated){
                return false;
            }

            $('#btn').blur();
            $('input').blur();
            $("#se-pre-con").show();
            form.off('submit');
            form.submit(); 
        });
        $("a[name='deleteButton']").click(function(){
            $("#se-pre-con").show();
            setTimeout(function(){
                $("#se-pre-con").hide();
                $('.delete-form').on('submit', function(e){
                    var form = $(this);
                    e.preventDefault();
                    $('#btn1').blur();
                    $('input').blur();
                    $("#se-pre-con").show();
                    form.off('submit');
                    form.submit(); 
                });
            }, 100)
        });
    });  
