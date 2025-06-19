var TrackComplaint = function () {
    var init = function () {
        loadTrackGrid();
    };

    var loadTrackGrid = function () {
        $.ajax({
            url: '/ClientTrack/GetComplaintTrackBasic',
            type: 'GET',
            success: function (data) {
                var tbody = $('#tblTrack tbody');
                tbody.empty();

                if (data && data.length > 0) {
                    $.each(data, function (i, item) {
                        var img = item.AttachmentPath
                            ? '<a href="' + item.AttachmentPath + '" target="_blank"><img src="' + item.AttachmentPath + '" style="height:40px;" /></a>'
                            : 'No Attachment';

                        var row = '<tr>' +
                            '<td>' + item.Title + '</td>' +
                            '<td>' + item.Category + '</td>' +
                            '<td>' + item.CPriority + '</td>' +
                            '<td>' + item.CStatus + '</td>' +
                            '<td>' + (item.AssignedAdmin || '-') + '</td>' +
                            '<td>' + item.AssignDate + '</td>' +
                            '<td>' + (item.AssignedTechnician || '-') + '</td>' +
                            '<td>' + item.CompleteDate + '</td>' +
                            '<td>' + (item.ResolutionRemarks || '-') + '</td>' +
                            '<td>' + img + '</td>' +
                            '<td>' + item.CreatedDate + '</td>' +
                            '</tr>';
                        tbody.append(row);
                    });
                } else {
                    tbody.append('<tr><td colspan="9" class="text-center">No data found</td></tr>');
                }
            },
            error: function () {
                alert("Error loading complaint data.");
            }
        });
    };

    return {
        Init: init
    };
}();

$(document).ready(function () {
    TrackComplaint.Init();
});
