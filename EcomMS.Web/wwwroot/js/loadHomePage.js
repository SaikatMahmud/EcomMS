$(document).ready(function () {
    $('#categoryId').change(function () {
        var selectedValue = $('#categoryId').val();
        window.location.href = `/Home/Index?filterbycat=${selectedValue}`;
    });
});