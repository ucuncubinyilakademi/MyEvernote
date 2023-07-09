var noteid = -1;
var modelCommentBodyId = "#modal_comment_body";

$(function () {
    $('#modal_comment').on('show.bs.modal', function (e) {
        var btn = $(e.relatedTarget);
        noteid = btn.data("note-id")

        $(modelCommentBodyId).load("/Comment/ShowNoteComments/" + noteid)
    });
})


function doComment(btn, e, commentid, spanid) {
    var button = $(btn);
    var mode = button.data("edit-mode")

    if (e === "edit_clicked") {
        if (!mode) {
            button.data("edit-mode", true);
            button.removeClass("btn-warning");
            button.addClass("btn-success");
            var btnSpan = button.find("span");
            btnSpan.removeClass("glyphicon-edit");
            btnSpan.addClass("glyphicon-ok");

            $(spanid).attr("contenteditable", true);
        }
        else {
            button.data("edit-mode", false);
            button.addClass("btn-warning");
            button.removeClass("btn-success");
            var btnSpan = button.find("span");
            btnSpan.addClass("glyphicon-edit");
            btnSpan.removeClass("glyphicon-ok");

            $(spanid).attr("contenteditable", false);

            var txt = $(spanid).text();
            $.ajax({
                method: "POST",
                url: "/Comment/Edit/" + commentid,
                data: { text: txt }
            }).done(function (data) {
                if (data.result) {
                    // yorumlar partial tekrar yüklenir.
                    $(modelCommentBodyId).load("/Comment/ShowNoteComments/" + noteid)
                }
                else {
                    alert("Güncelleme Başarısız...")
                }
            }).fail(function () {
                alert("Sunucu ile bağlantı kurulamadı.")
            })
        }
    }

    else if (e === "delete_clicked") {
        var dialog_res = confirm("Yorum silinsin mi?");
        if (!dialog_res) return false;

        $.ajax({
            method: "GET",
            url: "/Comment/Delete/" + commentid
        }).done(function (data) {
            if (data.result) {
                // yorumlar partial tekrar yüklenir.
                $(modelCommentBodyId).load("/Comment/ShowNoteComments/" + noteid)
            }
            else {
                alert("Yorum Silinemedi.")
            }
        }).fail(function () {
            alert("Sunucu ile bağlantı kurulanamdı.")
        })
    }
    else if (e === "new_clicked") {
        var txt = $("#new_comment_text").val();
        $.ajax({
            method: "POST",
            url: "/Comment/Create",
            data: { "text": txt, "noteid": noteid }
        }).done(function (data) {
            if (data.result) {
                // yorumlar partial tekrar yüklenir.
                $(modelCommentBodyId).load("/Comment/ShowNoteComments/" + noteid)
            }
            else {
                alert("Yorum Eklenemedi.")
            }
        }).fail(function () {
            alert("Sunucu ile bağlantı kurulanamdı.")
        })
    }
}
