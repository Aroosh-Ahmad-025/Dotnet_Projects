var readm = $('#readm');
var readcom = $("[id='readcom']");
var recid = "";
var LoggedinId = $("#loginid").val();
var msgcount = 0;
var commentcount = 0;
var prev = 0;



if (readm.text() != "") {
    msgcount = parseInt(readm.text());
}

if (readcom.text() != "" && readcom!=null) {
    commentcount = parseInt(readcom.text());
}
//console.log(readm);


connection.on('newMessage', (sender, sndid, messageText, reciever, imgt, msg_id, ordid, filen) => {


    if (reciever == LoggedinId /*&& sndid== recid*/) {
       
        console.log("id="+recid);
        if ($(document).attr('title') != "IT Butler - Messages" && ordid == "") {
            msgcount++;
            readm.show();
            $("#readm").html("" + msgcount.toString() + "");
            //Unreadmsg(recid, msgcount);
        }

        else if ($(document).attr('title') != "IT Butler - Order Details" && ordid != "" || filen =="d_order") {

                commentcount++;

                //var getcount = UnreadCount(ordid, reciever);

                //var temp3 = parseInt(getcount);

                //var temp2 = temp3 + 1;

                readcom.text(commentcount);

                //UnreadComments(ordid, temp2, reciever);

        }


        if ($(document).attr('title') == "IT Butler - Orders" && ordid != "")
        {
            var getdiv = $("[id='msgzcount_" + ordid + "']").text();
            var cmt = 0;
            if (getdiv!="") {
                cmt = parseInt(getdiv);
            }
            cmt = cmt + 1;
            $("[id='msgzcount_"+ordid+"']").text(cmt.toString());

        }


       }
});

function Unreadmsg(id, cnt) {
    $.ajax({
        type: 'POST',
        url: "../Ajax/UnreadChat",
        data: { id: id, cnt: cnt },
        dataType: "json",
        success: function (response) {
        },
        error: function (response) {
            alert("Error");
        },
    });
}


function UnreadComments(id, cnt, id2) {
    $.ajax({
        type: 'POST',
        url: "../Ajax/UnreadComments",
        data: { id: id, cnt: cnt, UserId: id2 },
        dataType: "json",
        success: function (response) {
        },
        error: function (response) {
            alert("Error");
        },
    });
}



function UnreadCount(id, id2)
{
    var val = "";
    $.ajax({
        type: 'POST',
        url: "../Ajax/GetUnreadCountById",
        data: { OrderId: id, UserId: id2 },
        async: false,
        dataType: "json",
        success: function (response) {
            val = response;
        },
        error: function (response) {
            alert("Error");
        },
    });
    return val;
}






