function initSaveButton() {
    $("#save-btn").bind({
        "click": function () {
            var id = $(this).attr("id") || '';
            var companyIdentifier = $("input[name=\"CompanyIdentifier\"]").val() || '';
            var lastname = $("input[name=\"LastName\"]").val();
            var firstName = $("input[name=\"FirstName\"]").val();
            var birthdate = $("input[name=\"Birthdate\"]").val() || '';
            $.ajax({
                method: "POST",
                url: "/Driver/edit",
                data: {
                    id: id,
                    companyIdentifier: companyIdentifier,
                    lastname: lastname,
                    firstName: firstName,
                    birthdate: birthdate
                },
                success: function (data) {
                }
            });
        }
    });
}