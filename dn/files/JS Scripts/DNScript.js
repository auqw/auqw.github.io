
function getDate() {
	const monthNames = ["January", "February", "March", "April", "May", "June",
	  "July", "August", "September", "October", "November", "December"
	];
	n =  new Date();
	document.getElementById("inputDate").innerHTML = monthNames[n.getMonth()] + " " + n.getDate() + ", " + n.getFullYear();
	document.body.scrollTop = 0;
	document.documentElement.scrollTop = 0;
}

function removeAllChildNodes(parent) {
    while (parent.firstChild) {
        parent.removeChild(parent.firstChild);
    }
}





function changePFP(name) {
	var pic = document.getElementById("DNProfile");
	pic.src = "./files/pfp/" + name +".png";

	var pfp = document.getElementById("inputName");
	pfp.innerHTML = name;
	// alert(name);

}

function changeBG(bg_name) {

	document.getElementById("profileBGborder").style.backgroundImage = "url(./files/bg/" + bg_name;
	console.log("url(./files/pfp/" + name+ ".png)")
}

function closeModal_() {
	var modal = document.getElementById("myModal");
	modal.style.display = "none";



}