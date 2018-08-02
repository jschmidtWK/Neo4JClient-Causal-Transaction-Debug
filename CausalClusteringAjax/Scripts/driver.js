function initSaveButton() {
    $("#save-btn").bind({
        "click": function () {
            var id = $("input[name=\"Id\"]").val();
            var companyIdentifier = $("input[name=\"CompanyIdentifier\"]").val() || '';
            var lastname = $("input[name=\"Lastname\"]").val();
            var Firstname = $("input[name=\"Firstname\"]").val();
            var neoUrl = $("input[name=\"neoUrl\"]").val();
            var neoUser = $("input[name=\"neoUser\"]").val();
            var neoPort = $("input[name=\"neoPort\"]").val();
            var neoPassword = $("input[name=\"neoPassword\"]").val();
            console.log("firstName", Firstname);
            $.ajax({
                method: "POST",
                url: "/Driver/Edit",
                data: {
                    id: id,
                    companyIdentifier: companyIdentifier,
                    lastname: lastname,
                    firstname: Firstname,
                    neoUrl: neoUrl,
                    neoPassword: neoPassword,
                    neoUser: neoUser,
                    neoPort: neoPort
                },
                success: function (data) {
                    
                }
            });
            
        }
    });

    $("#save-btn2").bind({
        "click": function () {
            console.log("nsdhfgbh");
            var id = $("input[name=\"Id\"]").val();
            var companyIdentifier = $("input[name=\"CompanyIdentifier\"]").val() || '';
            var lastname = $("input[name=\"Lastname\"]").val();
            var Firstname = $("input[name=\"Firstname\"]").val();
            var neoUrl = $("input[name=\"neoUrl\"]").val();
            var neoUser = $("input[name=\"neoUser\"]").val();
            var neoPort = $("input[name=\"neoPort\"]").val();
            var neoPassword = $("input[name=\"neoPassword\"]").val();
            console.log("firstName", Firstname);
            $.ajax({
                method: "POST",
                url: "/Driver/Edit",
                data: {
                    id: id,
                    companyIdentifier: companyIdentifier,
                    lastname: lastname,
                    firstname: Firstname,
                    neoUrl: neoUrl,
                    neoPassword: neoPassword,
                    neoUser: neoUser,
                    neoPort: neoPort
                },
                success: function (data) {

                }
            });

        }
    });
}