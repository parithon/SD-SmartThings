var websocket = null,
    actionInfo = {},
    info = {},
    uuid = null,
    globalSettings = {};

function registerPluginOrPI(inEvent, inUUID) {
    if (websocket) {
        var json = {
            event: inEvent,
            uuid: inUUID
        };
        websocket.send(JSON.stringify(json));
    }
}

function requestGlobalSettings(inUUID) {
    if (websocket) {
        var json = {
            event: "getGlobalSettings",
            context: inUUID
        };
        websocket.send(JSON.stringify(json));
    }
}

function saveGlobalSettings(inUUID) {
    if (websocket) {
        var json = {
            event: "setGlobalSettings",
            context: inUUID,
            payload: globalSettings
        };
        websocket.send(JSON.stringify(json));
    }
}

function connectElgatoStreamDeckSocket(inPort, inUUID, inRegisterEvent, inInfo, inActionInfo) {
    actionInfo = JSON.parse(inActionInfo);
    info = JSON.parse(inInfo);
    uuid = inUUID;

    const sceneSelect = document.getElementById("scene-select");
    sceneSelect.addEventListener('change', (evt) => {
        var json = {
            action: actionInfo.action,
            event: "sendToPlugin",
            context: uuid,
            payload: {
                command: "select",
                sceneId: sceneSelect.value
            }
        };
        websocket.send(JSON.stringify(json));
    });

    websocket = new WebSocket(`ws://127.0.0.1:${inPort}`);

    websocket.onopen = () => {
        registerPluginOrPI(inRegisterEvent, inUUID);
        requestGlobalSettings(inUUID);
    }

    websocket.onmessage = (evt) => {
        var jsonObj = JSON.parse(evt.data);
        var event = jsonObj["event"];
        var jsonPayload = jsonObj["payload"];

        if (event === "didReceiveGlobalSettings") {
            globalSettings = jsonPayload["settings"];
            checkAuthToken();
        }

        if (event === "sendToPropertyInspector") {
            console.log(`sendToPropertyInspector: ${JSON.stringify(jsonPayload)}`);
            var scenes = jsonPayload.scenes;
            sceneSelect.innerHTML = `<option>Select a scene</option>`;
            sceneSelect.innerHTML += scenes.map(s => `<option value="${s.SceneId}" ${s.Selected ? "selected" : ""}>${s.SceneName}</option>`);
        }
    }
}

function checkAuthToken() {
    if (!globalSettings.hasOwnProperty("authToken")) {
        window.open("setup.html");
        return false;
    }
    return true;
}

function getCallbackFromWindow(authToken) {
    console.log(`authToken: ${authToken}`);
    if (websocket) {
        globalSettings.authToken = authToken;
        saveGlobalSettings(uuid);
    }
}

function refreshScenes() {    ;
    if (websocket && checkAuthToken()) {
        var json = {
            action: actionInfo.action,
            event: "sendToPlugin",
            context: uuid,
            payload: {
                command: "refresh"
            }
        };
        websocket.send(JSON.stringify(json));
    }
}
