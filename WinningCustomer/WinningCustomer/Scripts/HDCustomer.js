function GetUpgradeOfferDetails(type, userID) {
    $.ajax({
        url: "/HDCustomer/UpgradeOfferSection",
        type: "POST",
        data: { userName: userID, customerType: type },
    })
    .success(function (result) {
        if (result != null) {
            $("#divUpgradeOfferSection").html(result);
        }

    })
    .error(function (xhr, status, errorThrown) {
    });
}

function SaveProducts(userName) {
    var hdnSelectedOffers = $("#hdnSelectedOffers");
    if (hdnSelectedOffers != undefined && hdnSelectedOffers != "" && hdnSelectedOffers != null) {
        $.ajax({
            url: "/HDCustomer/SaveProducts",
            type: "POST",
            data: { userID: userName, selectedPrd: hdnSelectedOffers.val() },
        })
    .success(function (result) {
        if (result != undefined && result != null) {
            $("#lblCustomerType").text(result);
            $("#tbdSuccessMessage").show();
        }
    })
    .error(function (xhr, status, errorThrown) {
    });
    }
}

function SaveAvailableOffers(selectedProd) {
    $("#hdnSelectedOffers").val(selectedProd);
}





