$(() => { // validationexample.js
    document.addEventListener("keyup", e => {
        $("#modalstatus").removeClass(); //remove any existing css on div
        if ($("#EmployeeModalForm").valid()) {
            $("#modalstatus").attr("class", "text-success"); //green
            $("#modalstatus").text("");
            $("#actionbutton").prop("disabled", false);
        }
        else {
            $("#modalstatus").attr("class", "text-danger"); //red
            $("#modalstatus").text("Please fix the highlighted fields.");
            $("#actionbutton").prop("disabled", true);
        }
    });

    $("#EmployeeModalForm").validate({
        rules: {
            TextBoxTitle: { maxlength: 4, required: true, validTitle: true },
            TextBoxFirst: { maxlength: 25, required: true },
            TextBoxLast: { maxlength: 25, required: true },
            TextBoxEmail: { maxlength: 40, required: true, email: true },
            TextBoxPhone: { maxlength: 15, required: true, validPhone: true }
        },
        errorElement: "div",
        messages: {
            TextBoxTitle: {
                required: "This field is required.", maxlength: "Please enter 1-40 chars.", validTitle: "Mr., Ms., Mrs., or Dr."
            },
            TextBoxFirst: {
                required: "This field is required.", maxlength: "Please enter 1-40 chars."
            },
            TextBoxLast: {
                required: "This field is required.", maxlength: "Please enter 1-40 chars."
            },
            TextBoxPhone: {
                required: "This field is required.", maxlength: "Please enter a valid phone number.", validPhone: "Please enter a valid phone number."
            },
            TextBoxEmail: {
                required: "This field is required.", maxlength: "Please enter 1-40 chars.", email: "Please enter a valid email."
            }
        }
    }); //EmployeeModalForm.validate
    $.validator.addMethod("validTitle", (value) => { //custome rule
        return (value === "Mr." || value === "Ms." || value === "Mrs." || value === "Dr.");
    }, "");
    $.validator.addMethod("validPhone", (value) => {
        return value.match(/^((\+[1-9]{1,4}[ \-]*)|(\([0-9]{2,3}\)[ \-]*)|([0-9]{2,4})[ \-]*)*?[0-9]{3,4}?[ \-]*[0-9]{3,4}?$/);
    }, "");
});
