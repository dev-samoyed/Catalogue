let buttons = document.getElementsByClassName("btn-color");

for (let i = 0; i < buttons.length; i++) {
    let button = buttons[i];

    button.addEventListener("click", function () {
        let color = this.getAttribute("data-color");
        let div = document.getElementsByClassName("sidebar")[0];
        div.style.backgroundColor = color;
    });
}