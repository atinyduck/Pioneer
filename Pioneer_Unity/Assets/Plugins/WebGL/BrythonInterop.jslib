mergeInto(LibraryManager.library, {
    RunBrythonScript: function (scriptPtr) {
        var scriptString = UTF8ToString(scriptPtr);

        // Ensure Brython is loaded on the page
        if (typeof __BRYTHON__ === 'undefined') {
            console.error("Brython is not loaded on this page!");
            myGameInstance.SendMessage('BrythonParser', 'ParseAndEnqueueCommands', '{"error": "Brython not loaded"}');
            return;
        }

        try {
            // We use standard JS evaluation to call a Brython wrapper function
            // In your index.html, you'll need a Javascript function that handles calling the Python runtime
            if (typeof window.executePlayerCode === 'function') {
                var jsonResult = window.executePlayerCode(scriptString);

                // Send the result back to our C# Parser
                myGameInstance.SendMessage('BrythonParser', 'ParseAndEnqueueCommands', jsonResult);
            } else {
                console.error("executePlayerCode JS bridge function is missing in index.html!");
            }
        } catch (e) {
            console.error("Error executing python script link:", e);
            myGameInstance.SendMessage('BrythonParser', 'ParseAndEnqueueCommands', JSON.stringify({error: e.message}));
        }
    }
});