var selectedCat = null;
var searchValue = "";
$(document).ready(function () {
    $('#categoryId').change(function () {
        $("#searchText").val("");
        searchValue = "";
        selectedCat = $('#categoryId').val();
        currentPage = 1;
        LoadHomePage();
    });

    $('#btnSearch').click(function () {
        $('#categoryId').val(0);
        selectedCat = null;
        searchValue = $('#searchText').val();
        currentPage = 1;
        LoadHomePage();
    });

});


function renderHomePage(obj) { //handlebar
    debugger;
    const template = $("#homePageProducts").html();
    const compiledTemplate = Handlebars.compile(template);
    debugger;
    const html = compiledTemplate({ product: obj });
    $(".render_homePage").html(html);
};

function LoadHomePage() {
    if (selectedCat == 0) {
        selectedCat = null;
    }
    console.log("executed");
    $.ajax({
        url: `/Home/GetHomePageProducts?filterbycat=${selectedCat}&search=${searchValue}&page=${currentPage}`,
        type: 'GET',
        success: function (response) {
            debugger;
            if (response.data && response.data.length > 0) { //handlebar
                renderHomePage(response.data);
                renderPaginationControls(currentPage, response.totalPage);
            } else {
                $(".render_homePage").html("<h2>Sorry! No product found.</h2>");
                renderPaginationControls(currentPage, response.totalPage);
            }
        },
        error: function (error) {
            alert("Internal server error!");
        }
    });
};

var currentPage = 1;
function goToPage(page) {
    currentPage = page;
    LoadHomePage();
}


function renderPaginationControls(currentPage, totalPages) {
    let pageNumberRender = `<ul class="pagination justify-content-center">`;
    // Previous button
    if (currentPage > 1) {
        pageNumberRender += `<li class="page-item disabled"><button class="page-link" onclick = "goToPage(${currentPage - 1})" tabindex="-1"><</button></li>`;
    }
    // Page numbers (all available page)
    for (let i = 1; i <= totalPages; i++) {
        if (i === currentPage) {
            pageNumberRender += `<li class="page-item active"><button class="page-link" onclick = "goToPage(${i})">${i}<span class="sr-only"></span></button></li>`;
        } else {
            pageNumberRender += `<li class="page-item"><button class="page-link" onclick = "goToPage(${i})">${i}</button></li>`;
        }
    }
    // Next button
    if (currentPage < totalPages) {
        pageNumberRender += `<li class="page-item"><button class="page-link" onclick = "goToPage(${currentPage + 1})">></button></li>`;
    }
    document.getElementById('HomepagePagination-controls').innerHTML = pageNumberRender;
}
