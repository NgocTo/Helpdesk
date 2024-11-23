$(() => { // main jQuery routine - executes every on page load, $ is short for jquery
    const getAll = async (msg) => {
        try {
            $("#employeeList").text("Finding Employee Information...");
            let response = await fetch(`api/employee`);
            if (response.ok) {
                let payload = await response.json(); // this returns a promise, so we await it
                sessionStorage.setItem("allemployees", JSON.stringify(payload));
                buildEmployeeList(payload);
                msg === "" ? // are we appending to an existing message
                    $("#status").text("Employees Loaded") : $("#status").text(`${msg} - Employees Loaded`);
            } else if (response.status !== 404) { // probably some other client side error
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else { // else 404 not found
                $("#status").text("no such path on server");
            } // else
            // get department data
            response = await fetch(`api/department`);
            if (response.ok) {
                let deps = await response.json(); // this returns a promise, so we await it
                sessionStorage.setItem("alldepartments", JSON.stringify(deps));
            } else if (response.status !== 404) { // probably some other client side error
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else { // else 404 not found
                $("#status").text("no such path on server");
            } // else
        } catch (error) {
            $("#status").text(error.message);
        }
    }; // getAll

    const buildEmployeeList = (data) => {
        $("#employeeList").empty();
        div = $(`
            <div class="list-group-item">
                <div class="row align-items-center">
                    <div class="col-6 col-md-8" id="status">Employee Info</div>
                    <div class="col-6 col-md-4 d-flex justify-content-end">
                        <button id="addEmployee" class="btn d-flex gap-1 btn-outline-dark"><span class="material-symbols-outlined">person_add</span> <span class="d-none d-md-block">New Employee</span></button>
                    </div>
                </div>
            </div>
        `);
        div.appendTo($("#employeeList"));
        let alldepartments = JSON.parse(sessionStorage.getItem('alldepartments'));
        data.forEach(emp => {
            let staffPicture = (emp.staffPicture64 === null) ?
                `<div class="bg-blue-dark initial-icon" style="background-color:${randomColor(emp.id)};">${emp.firstname.charAt(0)}${emp.lastname.charAt(0)}</div>` :
                `<img class="userimage-icon" src="data:img/png;base64,${emp.staffPicture64}" alt="${emp.firstname}">`;

            btn = $(`
                <div class="list-group-item" id="${emp.id}">
                    <div class="row align-items-center">
                        <div class="col-2 col-md-1" id="employeepicture${emp.id}">${staffPicture}</div>
                        <div class="col-10 col-md-9">
                            <div class="row">
                                <div class="col-12 col-md-5">
                                    <div>${emp.title} ${emp.firstname} ${emp.lastname}</div> 
                                    <div><span class="material-symbols-outlined title-icon">grid_view</span> ${emp.departmentName}</div> 
                                </div>
                                <div class="col-12 col-md-7">
                                    <div><span class="material-symbols-outlined title-icon">mail</span>${emp.email}</div> 
                                    <div><span class="material-symbols-outlined title-icon">tag</span>${emp.phoneno }</div> 
                                </div>
                            </div>
                        </div>
                        <div class="col-12 col-md-2 d-flex gap-1 justify-content-end">
                            <button id="update-${emp.id}" class="btn btn-outline-yellow update"><span class="material-symbols-outlined">edit_square</span></button>
                            <button id="delete-${emp.id}" class="btn btn-outline-danger delete"><span class="material-symbols-outlined">delete</span></button>
                        </div>
                    </div>
                </div>
                `
            );
            btn.appendTo($("#employeeList"));
        }); // forEach
    }; // buildEmployeeList


    getAll(""); // first grab the data from the server

    $("#employeeList").on('click', (e) => {
        const element = e.target;
        let data = JSON.parse(sessionStorage.getItem("allemployees"));
        if (e.target.closest("#addEmployee")) {
            setupForAdd();
        } else if (e.target.closest(".update")) {
            const id = e.target.closest(".update").id.split('-')[1];
            setupForUpdate(id, data);
        } else if (e.target.closest(".delete")) {
            const id = e.target.closest(".delete").id.split('-')[1];
            setupForDelete(id, data);
        }
    }); // employeeListClick

    $("#searchEmployee").on('input', (e) => {
        const data = JSON.parse(sessionStorage.getItem("allemployees")) || [];
        const searchValue = $("#searchEmployee").val().toLowerCase();
        if (searchValue !== "") {
            const filteredEmployees = data.filter(employee =>
                employee.firstname.toLowerCase().includes(searchValue) ||
                employee.lastname.toLowerCase().includes(searchValue)
            );
            buildEmployeeList(filteredEmployees);
        } else {
            buildEmployeeList(data);
        }
    });

    const add = async () => {
        try {
            emp = new Object();
            emp.title = $("#TextBoxTitle").val();
            emp.firstname = $("#TextBoxFirst").val();
            emp.lastname = $("#TextBoxLast").val();
            emp.email = $("#TextBoxEmail").val();
            emp.phoneno = $("#TextBoxPhone").val();
            emp.departmentId = parseInt($("#ddlDepartments").val());
            emp.staffPicture64 = sessionStorage.getItem("picture");
            emp.id = -1;
            emp.timer = null;
            // send the employee info to the server asynchronously using POST
            let response = await fetch("api/employee", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json; charset=utf-8"
                },
                body: JSON.stringify(emp)
            });
            if (response.ok) // or check for response.status
            {
                let data = await response.json();
                getAll(data.msg);
            } else if (response.status !== 404) { // probably some other client side error
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else { // else 404 not found
                $("#status").text("no such path on server");
            } // else
        } catch (error) {
            $("#status").text(error.message);
        } // try/catch
        $("#theModal").modal("toggle");
    }; // add

    const update = async (e) => {
        // action button click event handler
        try {
            // set up a new client side instance of Employee
            let emp = JSON.parse(sessionStorage.getItem("employee"));
            // pouplate the properties
            emp.title = $("#TextBoxTitle").val();
            emp.firstname = $("#TextBoxFirst").val();
            emp.lastname = $("#TextBoxLast").val();
            emp.email = $("#TextBoxEmail").val();
            emp.phoneno = $("#TextBoxPhone").val();
            emp.departmentId = parseInt($("#ddlDepartments").val());
            if (sessionStorage.getItem("picture") !== null)
                emp.staffPicture64 = sessionStorage.getItem("picture");
            // send the updated back to the server asynchronously using Http PUT
            let response = await fetch("api/employee", {
                method: "PUT",
                headers: { "Content-Type": "application/json; charset=utf-8" },
                body: JSON.stringify(emp),
            });
            if (response.ok) {
                // or check for response.status
                let payload = await response.json();
                $("#status").text(payload.msg);
                getAll(payload.msg);
            } else if (response.status !== 404) {
                // probably some other client side error
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else {
                // else 404 not found
                $("#status").text("no such path on server");
            } // else
        } catch (error) {
            $("#status").text(error.message);
            console.table(error);
        } // try/catch

        $("#theModal").modal("toggle");
    }; // update

    const _delete = async () => {
        let employee = JSON.parse(sessionStorage.getItem("employee"));
        try {
            let response = await fetch(`api/employee/${employee.id}`, {
                method: 'DELETE',
                headers: { 'Content-Type': 'application/json; charset=utf-8' }
            });
            if (response.ok) // or check for response.status
            {
                let data = await response.json();
                getAll(data.msg);
            } else {
                $('#status').text(`Status - ${response.status}, Problem on delete server side, see server console`);
            } // else
        } catch (error) {
            $('#status').text(error.message);
        }
    }; // _delete

    const loadDepartmentDDL = (empdep) => {
        html = '';
        $('#ddlDepartments').empty();
        let alldepartments = JSON.parse(sessionStorage.getItem('alldepartments'));
        alldepartments.forEach((dep) => { html += `<option value="${dep.id}">${dep.departmentName}</option>` });
        $('#ddlDepartments').append(html);
        $('#ddlDepartments').val(empdep);
    }; // loadDepartmentDDL

    const clearModalFields = () => {
        loadDepartmentDDL(-1);
        $("#TextBoxTitle").val("");
        $("#TextBoxFirst").val("");
        $("#TextBoxLast").val("");
        $("#TextBoxEmail").val("");
        $("#TextBoxPhone").val("");
        $("#uploader").val("");
        // clean out the other four text boxes go here as well
        sessionStorage.removeItem("employee");
        sessionStorage.removeItem("picture");
        $("#theModal").modal("toggle");
        $("#uploadstatus").text("");
        $("#imageHolder").html("");
        let validator = $("#EmployeeModalForm").validate();
        validator.resetForm();
    }; // clearModalFields

    const setupForAdd = () => {
        $("#actionbutton").val("Add");
        $("#modaltitle").html("Add Employee");
        $("#theModal").modal("toggle");
        $("#theModalLabel").text("Add");
        clearModalFields();
    }; // setupForAdd

    const setupForUpdate = (id, data) => {
        $("#actionbutton").val("Update");
        $("#modaltitle").html("Update Employee");
        clearModalFields();
        data.forEach(employee => {
            if (employee.id === parseInt(id)) {
                $("#TextBoxTitle").val(employee.title);
                $("#TextBoxFirst").val(employee.firstname);
                $("#TextBoxLast").val(employee.lastname);
                $("#TextBoxEmail").val(employee.email);
                $("#TextBoxPhone").val(employee.phoneno);
                sessionStorage.setItem("employee", JSON.stringify(employee));
                loadDepartmentDDL(employee.departmentId);
                if (employee.staffPicture64 !== null)
                    $("#imageHolder").html(`<img height="100" width="100" src="data:img/png;base64,${employee.staffPicture64}">`);
                else
                    $("#imageHolder").html(`<div class="bg-blue-dark initial-icon" style="background-color:${randomColor(employee.id)};width:100px;height:100px;">${employee.firstname.charAt(0)}${employee.lastname.charAt(0)}</div>`);
                $("#theModalLabel").text("Update");
                $("#theModal").modal("toggle");
            } // if
        }); // data.forEach
    }; // setupForUpdate

    const setupForDelete = (id, data) => {

        data.forEach(employee => {
            if (employee.id === parseInt(id)) {
                sessionStorage.setItem("employee", JSON.stringify(employee));
                $("#confirmDelete .modal-body p").html(`<p>Are you sure you want to delete employee ${employee.firstname} ${employee.lastname}?</p>`);
                $("#confirmDelete").modal("toggle");
            } // if
        }); // data.forEach
    }; // setupForAdd

    $("#actionbutton").on("click", () => {
        $("#actionbutton").val() === "Update" ? update() : add();
    }); // actionbutton click

    $("#nobutton").on("click", (e) => {
        $("#confirmDelete").modal("toggle");
    });
    $("#yesbutton").on("click", () => {
        $("#confirmDelete").modal("toggle");
        _delete();
    });

    $("#uploader").on("change", () => {
        try {
            const reader = new FileReader();
            const file = $("#uploader")[0].files[0];
            $("#uploadstatus").text("");
            file ? reader.readAsBinaryString(file) : null;
            reader.onload = (readerEvt) => {
                // get binary data then convert to encoded string
                const binaryString = reader.result;
                const encodedString = btoa(binaryString);
                // replace the picture in session storage
                let employee = JSON.parse(sessionStorage.getItem("employee"));
                if (employee !== null) {
                    employee.picture64 = encodedString;
                    sessionStorage.setItem("employee", JSON.stringify(employee));
                }
                sessionStorage.setItem('picture', encodedString);
                $("#uploadstatus").text("Picture uploaded.")
                $("#imageHolder").html(`<img height="100" width="100" src="data:img/png;base64,${employee.picture64}">`);
            };
        } catch (error) {
            $("#uploadstatus").text("pic upload failed")
        }
    }); // input file change

    let hexArray = ['#5C3F3F', '#9B6303', '#585991', '#46756E', '#654C6E']
    let randomColor = (value) => { return hexArray[(value % hexArray.length)] };
}); // jQuery ready method