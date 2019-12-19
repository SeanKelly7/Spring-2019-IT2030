$(function () {
    $("#Checkout").click(function (e) {
        e.preventDefault();

        var count = parseInt($("#Count").val());
        var available = parseInt($("#Count").attr("data-available"));

        if(available >= count)
        {
            var Url = $(this).attr("href");
            var newUrl = Url + "?count=" + count;
            window.location.href = newUrl;
        }
        else
        {
            $("#error-message").html("<p>ERROR: Not enough tickets available!</p>");
        }
    })

    $(".cancelOrder").click(function () {
        var id = $(this).attr("data-id");
        var title = $(this).attr("data-title");

        $.post("/Order/CancelOrder", { "id": id }, function (data) {
            $("#item-status-" + data.DeleteId).text(data.Status);
            $("#item-cancel-" + data.DeleteId).text("");
            $("#update-message").text("Your order for " + title + " has been canceled");
        });
    })
});

function searchFailed() {
    $("#searchresults").html("<p>Sorry, there was a problem with the search.</p>");
}