window.onload = init;

function init() {
    alert("This is a test");

    document.getElementById("Route_Name").onchange = function () {
        alert("Route Name changed...");
    }
}