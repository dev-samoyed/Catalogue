//function changeColor() {
//    let buttons = document.getElementsByClassName("btn-color");

//    for (let i = 0; i < buttons.length; i++) {
//        let button = buttons[i];

//        button.addEventListener("click", function () {
//            let color = this.getAttribute("data-color");
//            let div = document.getElementsByClassName("sidebar")[0];
//            div.style.backgroundColor = color;
//        });
//    }
//}
//function GreenColor() {
//    document.getElementById("myDIV").style.backgroundColor = "lightblue";
//}


function myFunction() {
    document.getElementById("myDropdown").classList.toggle("show");
}

function filterFunction() {
    var input, filter, ul, li, a, i;
    input = document.getElementById("myInput");
    filter = input.value.toUpperCase();
    div = document.getElementById("myDropdown");
    a = div.getElementsByTagName("a");
    for (i = 0; i < a.length; i++) {
        if (a[i].innerHTML.toUpperCase().indexOf(filter) > -1) {
            a[i].style.display = "";
        } else {
            a[i].style.display = "none";
        }
    }
}

/*
<div class="content">
                    <button class="btn-color" data-color="azure" onclick="AzureColor()">1</button>
                    <button class="btn-color" data-color="lawngreen" onclick="lawngreenColor()">2</button>
                    <button class="btn-color" data-color="darkred" onclick="darkredColor()">3</button>
                    <button class="btn-color" data-color="cyan" onclick="cyanColor()">4</button>
                    <button class="btn-color" data-color="lightslategray" onclick="lightslategrayColor()">5</button>
                </div>
*/