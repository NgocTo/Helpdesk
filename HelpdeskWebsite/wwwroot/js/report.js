$(() => { // main jQuery routine - executes every on page load, $ is short for jquery
    const getAll = async (msg) => {
        try {
            $("#reportList").text("Finding Report Information...");
            let response = await fetch(`api/problem`);
            if (response.ok) {
                let payload = await response.json(); // this returns a promise, so we await it
                sessionStorage.setItem("allreports", JSON.stringify(payload));
                buildReportList(payload);
                msg === "" ? // are we appending to an existing message
                    $("#status").text("Reports Loaded") : $("#status").text(`${msg} - Reports Loaded`);
            } else if (response.status !== 404) { // probably some other client side error
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else { // else 404 not found
                $("#status").text("no such path on server");
            } // else
            // get department data
            response = await fetch(`api/department`);
            if (response.ok) {
                let divs = await response.json(); // this returns a promise, so we await it
                sessionStorage.setItem("alldepartments", JSON.stringify(divs));
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

    const buildReportList = (data) => {
        $("#reportList").empty();
        div = $(`
            <div class="list-group-item">
                <div class="row align-items-center">
                    <div class="col-8" id="status">Report Info</div>
                    <div class="col-4 d-flex justify-content-end">
                        <button id="addReport" class="btn d-flex btn-outline-dark"><span class="material-symbols-outlined">add</span> New Report</button>
                    </div>
                </div>
            </div>
        `);
        div.appendTo($("#reportList"));
        let alldepartments = JSON.parse(sessionStorage.getItem('alldepartments'));
        data.forEach(rep => {
            btn = $(`
                <div class="list-group-item" id="${rep.id}">
                    <div class="row align-items-center">
                        <div class="col-2 col-md-1" id="${rep.id}">#${rep.id}</div>
                        <div class="col-10 col-md-8">${rep.description}</div>
                        <div class="col-12 col-md-3 d-flex gap-1 justify-content-end">
                            <button id="update-${rep.id}" class="btn btn-outline-yellow update"><span class="material-symbols-outlined">edit_square</span></button>
                            <button id="delete-${rep.id}" class="btn btn-outline-danger delete"><span class="material-symbols-outlined">delete</span></button>
                        </div>
                    </div>
                </div>
                `
            );
            btn.appendTo($("#reportList"));
        }); // forEach
    }; // buildReportList



    getAll(""); // first grab the data from the server

    $("#reportList").on('click', (e) => {
        const element = e.target;
        let data = JSON.parse(sessionStorage.getItem("allreports"));
        if (e.target.closest("#addReport")) {
            setupForAdd();
        } else if (e.target.closest(".update")) {
            const id = e.target.closest(".update").id.split('-')[1];
            setupForUpdate(id, data);
        } else if (e.target.closest(".delete")) {
            const id = e.target.closest(".delete").id.split('-')[1];
            setupForDelete(id, data);
        }
    }); // reportListClick

    $("#searchReport").on('input', (e) => {
        const data = JSON.parse(sessionStorage.getItem("allreports")) || [];
        const searchValue = $("#searchReport").val().toLowerCase();
        if (searchValue !== "") {
            const filteredReports = data.filter(report =>
                report.description.toLowerCase().includes(searchValue)
            );
            buildReportList(filteredReports);
        } else {
            buildReportList(data);
        }
    });

    const add = async () => {
        try {
            rep = new Object();
            rep.description = $("#TextBoxDesc").val();
            rep.id = -1;
            rep.timer = null;
            // send the report info to the server asynchronously using POST
            let response = await fetch("api/problem", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json; charset=utf-8"
                },
                body: JSON.stringify(rep)
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
        try {
            let rep = JSON.parse(sessionStorage.getItem("report"));
            rep.description = $("#TextBoxDesc").val();
            let response = await fetch("api/problem", {
                method: "PUT",
                headers: { "Content-Type": "application/json; charset=utf-8" },
                body: JSON.stringify(rep),
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
        let report = JSON.parse(sessionStorage.getItem("report"));
        try {
            let response = await fetch(`api/problem/${report.id}`, {
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

    const clearModalFields = () => {
        $("#TextBoxDesc").val("");
        // clean out the other four text boxes go here as well
        sessionStorage.removeItem("report");
        $("#theModal").modal("toggle");
    }; // clearModalFields

    const setupForAdd = () => {
        $("#actionbutton").val("Add");
        $("#modaltitle").html("Add Report");
        $("#theModal").modal("toggle");
        $("#theModalLabel").text("Add");
        clearModalFields();
    }; // setupForAdd

    const setupForUpdate = (id, data) => {
        $("#actionbutton").val("Update");
        $("#modaltitle").html("Update Report");
        clearModalFields();
        data.forEach(report => {
            if (report.id === parseInt(id)) {
                $("#TextBoxDesc").val(report.description);
                sessionStorage.setItem("report", JSON.stringify(report));
                $("#theModalLabel").text("Update");
                $("#theModal").modal("toggle");
            } // if
        }); // data.forEach
    }; // setupForUpdate

    const setupForDelete = (id, data) => {

        data.forEach(report => {
            if (report.id === parseInt(id)) {
                sessionStorage.setItem("report", JSON.stringify(report));
                $("#confirmDelete .modal-body p").html(`<p>Are you sure you want to delete report: </p><p>#${report.id} - ${report.description}</p>`);
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
}); // jQuery ready method