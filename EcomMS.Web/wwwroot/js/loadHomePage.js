var selectedCat = null;
var searchValue = "";
$(document).ready(function () {
    $('#categoryId').change(function () {
        $("#searchText").val("");
        searchValue = "";
        selectedCat = $('#categoryId').val();
        LoadHomePage(selectedCat, searchValue);
    });

    $('#btnSearch').click(function () {
        $('#categoryId').val(0);
        selectedCat = null;
        searchValue = $('#searchText').val();
        LoadHomePage(selectedCat, searchValue);
    });

});




function renderHomePage(obj) { //handlebar
    debugger;
    const template = $("#homePageProducts").html();
    const compiledTemplate = Handlebars.compile(template);
    debugger;
    const html = compiledTemplate({ product : obj });
    $(".render_homePage").html(html);
};

function LoadHomePage(selectedCat, search) {
    if (selectedCat == 0) {
        selectedCat = null;
    }
    console.log("executed");
    $.ajax({
        //url: `/Home/Index?filterbycat=${selectedCat}&search=${search}&page=${page}`,
        url: `/Home/GetHomePageProducts?filterbycat=${selectedCat}&search=${searchValue}`,
        type: 'GET',
        success: function (response) {
            debugger;
            if (response.data && response.data.length > 0) {
                renderHomePage(response.data);
            } else {
                $(".render_homePage").html("<h2>Sorry! No product found.</h2>");
            }
        },
        error: function (error) {
            alert("Internal server error!");
        }
    });
};
