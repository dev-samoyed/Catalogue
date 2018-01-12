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
function AzureColor() {
    document.getElementsByClassName("sidebar").style.backgroundColor = "azure";
}
function lightslategrayColor() {
    document.getElementsByClassName("sidebar").style.backgroundColor = "lightslategray";
}
function lawngreenColor() {
    document.getElementsByClassName("sidebar").style.backgroundColor = "lawngreen";
}
function darkredColor() {
    document.getElementsByClassName("sidebar").style.backgroundColor = "darkred";
}
function cyanColor() {
    document.getElementsByClassName("sidebar").style.backgroundColor = "cyan";
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