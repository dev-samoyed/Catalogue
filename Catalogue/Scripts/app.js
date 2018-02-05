//on close collapse
$('.collapse').on('hidden.bs.collapse', function () {
  var target = '#'+$(this).attr('data-parent');
  $(target).removeClass('collapse-open');
});

//on open collapse
$('.collapse').on('shown.bs.collapse', function () {
    var target = '#'+$(this).attr('data-parent');
    $(target).addClass('collapse-open');
});


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

        reader.readAsDataURL(file);
        reader.onload = function (_file) {

            image.src = _file.target.result;
            image.onload = function () {

                $("#targetImg").attr('src', _file.target.result);
                $("#imgPreview").show();
            }
        }
    }
}

function ClearPreview () {
    $("#imageBrowes").val('');
    $("#imgPreview").hide();
}

function checkDismissed () {
    $("#dateDis").on('change', function () {
        var dis_date = $("#dateDis")[0];
        var checkbox = $("#checkBox")[0];
        if(dis_date.value != "")
        {
            checkbox.checked = true;
            checkbox.setAttribute("disabled", true);
        } else {
            checkbox.checked = false;
            checkbox.removeAttribute("disabled");
        }
    });

    $("#checkBox").on('click', function () {
        var dis_date = $("#dateDis")[0];
         
         if (this.checked == true) {
            var now = new Date();
            var day = ("0" + now.getDate()).slice(-2);
            var month = ("0" + (now.getMonth() + 1)).slice(-2);
            var today = now.getFullYear()+"-"+(month)+"-"+(day) ;

            dis_date.value = today;
            dis_date.setAttribute("readonly", true);
        } 
         else if(this.checked == false) {
             dis_date.value = null;
             dis_date.removeAttribute("readonly");
         }
        
    });
}

