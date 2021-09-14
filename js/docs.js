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


function loadNav(level) {
   document.getElementById("nav_holder").innerHTML=`
      <div id="mySidebar" class="sideNav">
         <div class="sideNavImage center"><a href="${level}/index" id="homeLink"><h1>AutoQuest Worlds</h1></a></div>
         <div id="sidelinks">
            <h3>CONTENTS:</h3>
            <a id="docs-tab" href="${level}/docs.html">Home</a>
            <a id="authors-tab" href="${level}/d/authors.html">Author</a>
            <a id="installation-tab" href="${level}/d/installation.html">Installation</a>
            <a id="versions-tab" href="${level}/d/versions.html">Versions</a>
            <a id="guide-tab" href="${level}/d/guide.html">Guide</a>

         </div>
      </div>`
}

            // <a id="create-tab" href="${level}/d/create.html">Create</a>
            // <a id="recommended-tab" href="${level}/d/recommended.html">Bots</a>
            // <a id="army-tab" href="${level}/d/army.html">Army</a>
            // <a id="quest-tab" href="${level}/d/quest.html">Quest List</a>

function loadNavGuide(activePage, level) {
      let div = document.getElementById("sidelinks");

      let pagebtn = document.getElementById("guide-tab");
      pagebtn.classList.add("collapsibleSide")
      
            // <a id="g-UI" href=${level}/d/g/UI.html>• UI</a>
      // Insert jump-tos
      let pageDrop =  document.createElement('div');
      pageDrop.className = "collapsibleContentSide collapse-2nd collapsibleOpen";
      pageDrop.innerHTML = `

            <a id="g-BM" class="collapsibleSide">• Bot</a>
            <div class="collapsibleContentSide collapse-3rd">
               <a id="g-BM-Tip">Tip</a>
               <a id="g-BM-Info">Info</a>
               <a id="g-BM-Combat">Combat</a>
               <a id="g-BM-Item">Item</a>
               <a id="g-BM-Map">Map</a>
               <a id="g-BM-Quest">Quest</a>
               <a id="g-BM-Misc">Misc</a>
               <a id="g-BM-Misc2">Misc 2</a>
               <a id="g-BM-Options">Options</a>
               <a id="g-BM-Client">Client</a>
               <a id="g-BM-Bots">Bots</a>
            </div>

            <a id="g-Tools" class="collapsibleSide">• Tools</a>
            <div class="collapsibleContentSide collapse-3rd">
               <a id="g-Tools-FastTravel">Fast Travel</a>
               <a id="g-Tools-LoadersGrabbers">Loaders/Grabbers</a>
               <a id="g-Tools-Hotkeys">Hotkeys</a>
               <a id="g-Tools-PuginManager">Pugin Manager</a>
               <a id="g-Tools-Cosmetics">Cosmetics</a>
               <a id="g-Tools-Bank">Bank</a>
               <a id="g-Tools-EyeDropper">Eye Dropper</a>
               <a id="g-Tools-Logs">Logs</a>
               <a id="g-Tools-Notepad">Notepad</a>
               <a id="g-Tools-SetFPS">Set FPS</a>
            </div>
            <a id="g-Packets" class="collapsibleSide">• Packets</a>
            <div class="collapsibleContentSide collapse-3rd">
               <a id="g-Packets-Sniffer">Sniffer</a>
               <a id="g-Packets-Spammer">Spammer</a>
               <a id="g-Packets-Tamperer">Tamperer</a>
            </div>
            <a id="g-options" href=${level}/d/g/UI.html>• Options</a>
            <a id="g-bank" href=${level}/d/g/UI.html>• Bank</a>
            <a id="g-plugins" href=${level}/d/g/UI.html>• Plugins</a>

            <a id="g-Others" class="collapsibleSide">• Others</a>
            <div class="collapsibleContentSide collapse-3rd">
               <a id="g-Others-more">More</a>
               <a id="g-Others-MapJump">Map Jump</a>
            </div>
            <a id="g-var" class="collapsibleSide">• Variables</a>
            <div class="collapsibleContentSide collapse-3rd">
               <a id="g-var-cmd">List of supported cmds</a>
            </div>
      `

      div.insertBefore(pageDrop, pagebtn.nextSibling);

      document.getElementById(activePage).classList.add("sideNavActive");

      document.getElementById(activePage).href = "#main";

}