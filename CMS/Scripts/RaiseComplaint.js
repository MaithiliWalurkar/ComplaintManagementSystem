var RaiseComplaint = function () {
    var init = function () {
        bindEvents();
        loadComplaintGrid();
    };

    var bindEvents = function () {
        $('#btnSubmit').click(function () {
            var isValid = validateForm();
            if (!isValid) return;

            var title = $('#txtTitle').val();
            var category = $('#ddlCategory').val();
            var priority = $('#ddlPriority').val();
            var description = $('#txtDescription').val();
            var fileData = $('#fileAttachment').get(0).files;

            var formData = new FormData();
            formData.append("txtTitle", title);
            formData.append("ddlCategory", category);
            formData.append("ddlPriority", priority);
            formData.append("txtDescription", description);

            if (fileData.length > 0) {
                formData.append("fileAttachment", fileData[0]);
            }

            $.ajax({
                url: '/ClientRaise/SubmitComplaint',
                type: 'POST',
                data: formData,
                contentType: false,
                processData: false,
                success: function (res) {
                    if (res === "Success") {
                        alert("Complaint submitted successfully.");
                        clearForm();
                        loadComplaintGrid(); // reload after insert
                    } else if (res === "Duplicate") {
                        alert("Complaint already exists with same details and is still open.");
                    } else {
                        alert("Something went wrong. Please try again.");
                    }
                },
                error: function () {
                    alert("Server error occurred.");
                }
            });
        });
    };

    var validateForm = function () {
        var title = $('#txtTitle').val().trim();
        var category = $('#ddlCategory').val();
        var priority = $('#ddlPriority').val();
        var description = $('#txtDescription').val().trim();

        if (title === "") {
            alert("Please enter complaint title.");
            return false;
        }

        if (category === "") {
            alert("Please select a category.");
            return false;
        }

        if (priority === "") {
            alert("Please select a priority.");
            return false;
        }

        if (description === "") {
            alert("Please enter complaint description.");
            return false;
        }

        return true;
    };

    var clearForm = function () {
        $('#txtTitle').val('');
        $('#ddlCategory').val('');
        $('#ddlPriority').val('');
        $('#txtDescription').val('');
        $('#fileAttachment').val('');
    };

    var loadComplaintGrid = function () {
        $.ajax({
            url: '/ClientRaise/GetComplaints',
            type: 'GET',
            success: function (data) {
                var tbody = $('#tblComplaints tbody');
                tbody.empty();

                if (data && data.length > 0) {
                    $.each(data, function (i, item) {
                        var attachmentHTML = item.AttachmentPath
                            ? '<a href="' + item.AttachmentPath + '" target="_blank"><img src="' + item.AttachmentPath + '" style="height:40px;width:auto;" /></a>'
                            : 'No Image';

                        var row = '<tr>' +
                            '<td>' + item.Title + '</td>' +
                            '<td>' + item.Category + '</td>' +
                            '<td>' + item.CPriority + '</td>' +
                            '<td>' + item.CStatus + '</td>' +
                            '<td>' + item.CreatedDate + '</td>' +
                            '<td>' + attachmentHTML + '</td>' +
                            '</tr>';
                        tbody.append(row);
                    });
                } else {
                    tbody.append('<tr><td colspan="6" class="text-center">No complaints found.</td></tr>');
                }
            },
            error: function () {
                alert("Failed to load complaints.");
            }
        });
    };

    return {
        Init: init
    };
}();

$(document).ready(function () {
    RaiseComplaint.Init();
});
