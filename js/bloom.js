// import { env } from 'process';

// New tab function
function openInNewTab(link) {
  window.open(link,'_blank');
}

 // Copy the text inside the text field
function copy2clipboard(tagID) {
  navigator.clipboard.writeText(document.getElementById(tagID).innerHTML.trim());
}

// Clock Function
function showTime(datespan, timespan) {
  let datetime = new Date();
  let time = datetime.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric', second: 'numeric', hour12: true ,timeZone: 'America/New_York', timeZoneName:'short'});
  let date = datetime.toLocaleString("en-US", { timeZone: "America/New_York", dateStyle: 'long'}); 

  document.getElementById(timespan).innerHTML = time;
  document.getElementById(datespan).innerHTML = date;
}

function openModal(modal) {
  document.getElementById(modal).style.display = "block";
  // document.style.html = "overflow: -moz-scrollbars-none;scrollbar-width: none;-ms-overflow-style: none;";
}

function closeModal(modal) {
  document.getElementById(modal).style.display = "none";
  // console.log(env.GAY);
}


function sendFeedback() {
  let validation = validateForm();
  if (!validation) {
    return;
  }
  var radios = document.getElementsByName('stars');

  for (var i = 0, length = radios.length; i < length; i++) {
    if (radios[i].checked) {
      var rating = radios[i].value;
      break;
    }
  }
  console.log(rating);
  if (typeof rating == 'undefined') {
    alert("Must give a rating.");
    return;
  }
  var nameValue = document.getElementById("fname").value;
  var boatValue = document.getElementById("bname").value;
  var feedbackValue = document.getElementById("feedbackarea").value;
  // alert(nameValue + " " + boatValue + " " + feedbackValue + " " + rating);

  if (nameValue == "") {
    nameValue = "Anonymous Joe"
  }
  sendToDiscord(nameValue, boatValue, feedbackValue, rating);
  // Cleans Form
  let elements = document.getElementsByTagName("input");
  for (var i = 0; i < elements.length; i++) {
          if (elements[i].type == "radio") {
              elements[i].checked = false;
          }
      }
  closeModal('modalFeedback');
  document.getElementById("fname").value = "";
  document.getElementById("bname").value = "";
  document.getElementById("feedbackarea").value = "";
}

function sendToDiscord(nameValue, boatValue, feedbackValue, rating) {
  var request = new XMLHttpRequest();
  request.open("POST", "https://discord.com/api/webhooks/880850443230666832/XFRfTGBl1lbNy3P4_PU4zdywfu12CZ9q40eeSfCKrHoBJDjxl2G1KXZ_Oj_YlRHk4-_M");
  // again, replace the url in the open method with yours
  request.setRequestHeader('Content-type', 'application/json');
  let star = "⭐".repeat(rating);
  let desc = feedbackValue.match(/.{1,1020}/g);
  var myEmbed = {
    author: {
      name: nameValue
    },
    title: "Feedback Form",
    url: "https://auqw.tk/",
    description: `📌 **Botname:** \`${boatValue}\`\n📌 **Rating**: ${star}`,
    color: hexToDecimal("#F33F35"),
    fields: [
        {
          "name": "✉️ Message: ",
          "value": desc[0]
        }
    ]
  }

  
  for (var i = 1; i < desc.length; i++) {
      myEmbed.fields.push({
          "name": "\u200b",
          "value": desc[i]
      })
  }


  var params = {
    username: "Captain Coomer Joe",
    avatar_url: "https://cdn.discordapp.com/attachments/806920895934365766/881156113498771466/cb570fcf52d42d4a826c4f2a022ba381_-_Copy.jpg",
    embeds: [myEmbed]
  }

  request.send(JSON.stringify(params));
}

// function that converts a color HEX to a valid Discord color
function hexToDecimal(hex) {
  return parseInt(hex.replace("#",""), 16)
}
// Validates feedbacks
function validateForm() {
  if (document.forms["feedbackForm"]["bname"].value == "") {
    alert("Bot Name must be filled out.");
    return false;
  }
  if (document.forms["feedbackForm"]["feedbackarea"].value == "") {
    alert("Feedback must be filled out.");
    return false;
  }
  if (document.forms["feedbackForm"]["feedbackarea"].value.length >= 2000) {
    alert("Please limit your feedback message to 2000 characters.");
    return false;
  }
  return true;
}

// Opens and closes the tab
function openTab(evt, tabName, tabContent, tabLinks) {
  var i, tabcontent, tablinks;
  tabcontent = document.getElementsByClassName(tabContent);
  for (i = 0; i < tabcontent.length; i++) {
    tabcontent[i].style.display = "none";
  }
  tablinks = document.getElementsByClassName(tabLinks);
  for (i = 0; i < tablinks.length; i++) {
    tablinks[i].className = tablinks[i].className.replace(" active", "");
  }
  document.getElementById(tabName).style.display = "block";
  evt.currentTarget.className += " active";
}

function openFeedback(botname) {
   document.getElementById("bname").value = botname;
   openModal('modalFeedback');
}