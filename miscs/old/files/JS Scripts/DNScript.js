
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



// Define the function 
// to screenshot the div
function takeshot() {
	// window.scrollTo(0,0);
	// window.scroll({
	//    top: 0,
	//    left: 0,
	//    behavior: 'smooth'
	// });
	// document.body.scrollTop = 0;
	// document.documentElement.scrollTop = 0;

    document.getElementById("myNicPanel").style.display = 'none';
    document.getElementById("OutputName").style.display = '';
    
    document.getElementById("content-box-center").style.margin = '0 0 0 0';
    var div = document.getElementById('photo');

	let menu = document.getElementById('output');
	while (menu.firstChild) {
	    menu.removeChild(menu.firstChild);
	}    
	 

    html2canvas(div, {
    	allowTaint : true,
    	letterRendering: 1,
    	scale: 3,
    	dpi: 144,
    	useCORS: true,   
    	// foreignObjectRendering: true
    	backgroundColor:null, 
    	// imageSmoothingEnabled=true,
        // imageTimeout: 0;
        scrollX: -window.scrollX,
        scrollY: -window.scrollY,
        windowWidth: document.documentElement.offsetWidth,
        windowHeight: document.documentElement.offsetHeight,
    	}).then(
        function (canvas) {
        	canvas.id = "result";
        	canvas.crossorigin="anonymous"
            document.getElementById('output').appendChild(canvas);
            canvas.crossorigin="anonymous"
			var image = canvas.toDataURL("image/png").replace("image/png", "image/octet-stream");  // here is the most important part because if you dont replace you will get a DOM 18 exception.
			console.log(image)
			window.location.href=image; // it will save locally

			  // var link = document.getElementById('screenSave');
			  // link.setAttribute('download', 'MintyPaper.png');
			  // link.setAttribute('href', canvas.toDataURL("image/png").replace("image/png", "image/octet-stream"));
			  // link.click();
        })
    // div.display = ''
    document.getElementById("myNicPanel").style.display = '';
    document.getElementById("content-box-center").style.margin = 'auto';
    // var elmnt = document.getElementById("bottom-view");



    // elmnt.scrollIntoView(true); // Top
}

function changePFP(name) {
	var pic = document.getElementById("DNProfile");
	pic.src = "./files/pfp/" + name +".webp";

	var pfp = document.getElementById("inputName");
	pfp.innerHTML = name;
	// alert(name);
   
}

function changeBG(bg_name) {

	document.getElementById("profileBGborder").style.backgroundImage = "url(./files/bg/" + bg_name;
	console.log("url(./files/pfp/" + name+ ".webp)")
}

function closeModal_() {
	var modal = document.getElementById("myModal");
	modal.style.display = "none";



}