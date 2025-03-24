window.onload = init;

function init() {
    document.getElementById("filterVN").onkeyup = myFunction;
}

function myFunction() {
    //alert("this is a test");
    // Declare variables
    var input, filter, table, tr, td, i, txtValue;
    input = document.getElementById("filterVN");
    filter = input.value;
    table = document.getElementById("tblVehList");
    tr = table.getElementsByTagName("tr");

    // Loop through all table rows, and hide those who don't match the search query
    for (i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[0];
        if (td) {
            txtValue = td.textContent || td.innerText;
            if (txtValue.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
}