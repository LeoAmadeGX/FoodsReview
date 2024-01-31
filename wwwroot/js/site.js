// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function setupAutocomplete() {
    var availableRecorders = [ "Leo", "長勝", "Allen", "Top", "韋傑" ];

    $("#recorderInput").autocomplete({
        source: availableRecorders
    });
}

// 在頁面加載完成後調用自動完成的設置函數
$(document).ready(function() {
    setupAutocomplete();
});