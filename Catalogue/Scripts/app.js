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

        $(document).ready(function(){
            $('form').submit(function(e){
                var form = $(this);
                e.preventDefault();

                if (!$(".validation-span").is(":empty")) {
                    $("#se-pre-con").hide();
                    console.log("not valid");
                    return false;
                } else {
                    $('input, select').focus(function() {
                        console.log("validation ok");
                        this.blur();
                        form.submit(); 
                        return true;
                    });
                    
                }
                console.log("this");
                //$("#se-pre-con").show();
                       
            });
        });