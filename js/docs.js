function openNav() {
   // document.getElementById("mySideOpener").style.display = "none";
   console.log(document.documentElement.clientWidth)
   if (document.getElementById("main").style.marginLeft == "15em") {
      document.getElementById("mySidebar").style.width = "0";
      document.getElementById("main").style.marginLeft = "0";
   } else {
      document.getElementById("mySidebar").style.width = "15em";
      document.getElementById("main").style.marginLeft = "15em";
   }
}


function loadNav1() {
   document.getElementById("nav_holder").innerHTML=`
      <div id="mySidebar" class="sideNav">
         <div class="sideNavImage center"><a href="./index.html" id="homeLink"><h1>AutoQuest Worlds</h1></a></div>
         <div id="sidelinks">
            <h3>CONTENTS:</h3>
            <a id="docs-tab" href="./docs">Home</a>
            <a id="authors-tab" href="./d/authors.html">Author</a>
            <a id="installation-tab" href="./d/installation.html">Installation</a>
            <a id="versions-tab" href="./d/versions.html">Versions</a>

         </div>
      </div>`
}
            // <a id="guide-tab" href="./d/guide.html">Guide</a>
            // <a id="create-tab" href="./d/create.html">Create</a>
            // <a id="recommended-tab" href="../d/recommended.html">Bots</a>
            // <a id="army-tab" href="./d/army.html">Army</a>
            // <a id="quest-tab" href="./d/quest.html">Quest List</a>

function loadNav2() {
   document.getElementById("nav_holder").innerHTML=`
      <div id="mySidebar" class="sideNav">
         <div class="sideNavImage center"><a href="../index.html" id="homeLink"><h1>AutoQuest Worlds</h1></a></div>
         <div id="sidelinks">
            <h3>CONTENTS:</h3>
            <a id="docs-tab" href="../docs">Home</a>
            <a id="authors-tab" href="../d/authors.html">Author</a>
            <a id="installation-tab" href="../d/installation.html">Installation</a>
            <a id="versions-tab" href="../d/versions.html">Versions</a>

         </div>
      </div>`
}
            // <a id="guide-tab" href="../d/guide.html">Guide</a>
            // <a id="create-tab" href="../d/create.html">Create</a>
            // <a id="recommended-tab" href="../d/recommended.html">Bots</a>
            // <a id="army-tab" href="../d/army.html">Army</a>
            // <a id="quest-tab" href="../d/quest.html">Quest List</a>