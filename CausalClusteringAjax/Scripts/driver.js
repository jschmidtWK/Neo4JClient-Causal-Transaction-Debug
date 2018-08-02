function initSaveButton() {
    $("#save-btn").bind({
        "click": function () {
            var id = $(this).attr("id") || '';
            var companyIdentifier = $("#driver-edit-" + id + " input[name=\"Driver.CompanyIdentifier\"]").val() || '';
            var lastname = $("#driver-edit-" + id + " input[name=\"Driver.LastName\"]").val();
            var firstName = $("#driver-edit-" + id + " input[name=\"Driver.FirstName\"]").val();
            var birthdate = $("#driver-edit-" + id + " input[name=\"Driver.Birthdate\"]").val() || '';
            $.ajax({
                method: "POST",
                url: "/drivers/edit",
                data: {
                    id: id,
                    companyIdentifier: companyIdentifier,
                    lastname: lastname,
                    firstName: firstName,
                    birthdate: birthdate
                },
                success: function (data) {
                }
            }
                    }
    });
}