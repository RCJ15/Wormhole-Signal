# Thinking With Wormholes And Signals
Wormhole Signal is a tool for Unity which creates events that are tied to scriptable objects.

This means that you no longer have to create dependancies on scripts when wanting to receive certain events, which often leads to a phenomenon known as "Spaghetti Code". Using Wormhole Signal you can instead reference scriptable objects instead of any script you wrote, removing that unnecessary entanglement you would otherwise face.

# How To Install
1. Download the repository.
2. Place the unzipped folder anywhere in your Unity Assets folder.
3. You're now done and ready to use Wormhole Signals. Yep it's that simple.

# How To Transmit Through Wormholes
Each event (Signal) you want to create will be a scriptable object placed somewhere in your projects assets folder. For this I recommend that you place your Signals in their own folder if you intend to use this a lot.
So to get started, you create a new Signal scriptable object by right clicking in your projects tab and go to "Create/Wormhole Signal", you can then name your signal to whatever that event is going to be.

![Creating a Wormhole Signal Asset and renaming it](https://user-images.githubusercontent.com/73841786/223550621-8bbd3e22-b3b5-44f3-b4d2-06d08c3b519b.png)

You've now created your signal and it's now ready to be used in any script! Yep that's all that is needed.

# The Inspector
If you take a peek at the inspector of any Wormhole Signal that you've created, you'll see some non editable properties.

![Wormhole Signal Asset Inspector](https://user-images.githubusercontent.com/73841786/223552918-3d27a94f-17ff-4775-974d-9ed12e07905f.png)

The **GUID** is the Globally Unique IDentifier for that Signal. This ID will be unique for every single signal that is created. This value is actually assigned by Unity but each Signal saves it in their own variable so that they can utilize this value during builds.


The **Global Signal List** is a list of every single Signal in the entire project. This list is a direct view of the Signal List inspector and is used to get a Signal during runtime without needing to set a reference in the inspector. It's super important to note that this is not unique for each Signal and looks the same for every single Signal, hence why it's called the **Global Signal List** (emphasis on Global).


Lastly, the **"Find Global Signal List Object"** button will simply ping and automatically select the Signal List Asset in your project, which should be included when you download this package. This asset contains the Global Signal List as mentioned above.

# Global Signal List
As mentioned above, the Global Signal List is an asset that should always exist in your project. It should **always** exist, so if you've deleted this file accidentely, then please reinstall Wormhole Signal. This asset is responsible for keeping a list of every single Signal in the entire project. This is used during runtime to get a signal without needing to set a reference in the inspector. You do this via 2 ways.


**Way 1:** Get the Signal via Name.
Probably the easiest method if you plan on hardcoding it. Please note that the name **is** case sensitive. The code snippet below shows exactly how this can be achieved.

![Getting A Signal Via Name](https://user-images.githubusercontent.com/73841786/223554939-a7bf065f-fce8-415b-b209-7dcb2ddf7ea1.png)

**Way 2:** Get the Signal via GUID.
I don't really recommend this method unless you have some reference to the GUID that you store earlier. The code snippet below shows exactly how this can be achieved.

![Getting A Signal Via GUID](https://user-images.githubusercontent.com/73841786/223555057-e6ece94d-4b46-47f0-b89c-4e93591ef9c8.png)

## Why Use The Signal List?
Why should you even use this if it's 1000 times easier to just reference your signals in the inspector? Well, the Signal List is useful for when you **can't** set your Signal in the inspector.


**For example:** Say that you have a class that is static and unable to use any inspector of any sort. The solution to this would normally be to use FindObjectOfType or something alike to get a script which has a reference to a signal, however using the Signal List is much easier, looks cleaner in code and probably performs way better than FindObjectOfType.


But still, using the Signal List is very situational.

# How To Use Your Signal As An Event
Now that you've created your Wormhole Signal asset, how do we actually use it as an event? Here is a step by step guide on how to use the Signal.

**Step 1:** First you use the WormholeSignal namespace in your script.

![Using The WormholeSignal Namespace](https://user-images.githubusercontent.com/73841786/223557088-d867d758-64dc-48a0-97d0-638a631ab41c.png)

**Step 2:** You now need to create some sort of reference to your Signal. The easiet method is to simply create a public or private serialized field in the inspector so that you can simply drag the asset into it. You could also use the SignalList as shown earlier in the "Global Signal List" section of this text but that is not recommended for MonoBehaviours or any script with access to the inspector.

![Getting Signal References Through The Inspector](https://user-images.githubusercontent.com/73841786/223558266-58b0cb5a-1562-4311-8457-33fabc14641f.png)

**Step 3:** Now you need to subscribe and unsubscribe from the Signal using the Subscribe() and Unsubscribe() methods respectively. I recommend that you Subscribe in OnEnable and Unsubscribe in OnDisable if you're working in a MonoBehaviour script. Note that even methods that take in a single parameter work as well, meaning that you can send and receive values through Signals which is extremely useful! Note that ANY type of value is allowed, even a custom script that you created is allowed to be sent into a Signal!

![Subscribing And Unsubscribing From The Signal](https://user-images.githubusercontent.com/73841786/223561876-6f7f3873-90d0-4301-b135-f6d5f59e8764.png)

Do note that, when using your own data type when subscribing/unsubscribing, usage of the exact same Subscribe() and Unsubscribe() methods won't work, instead use the generic version of both of those methods.


**Step 4:** Call the Signal somewhere. It can be anywhere as long as you use the Call() method. Make sure to give a value in it if you want to send some custom values.

![Calling A Wormhole Signal](https://user-images.githubusercontent.com/73841786/223564325-b3978a2b-a831-4a0d-9edf-04e853bc89dd.png)

And that's it, you should now be ready to use Wormhole Signal for whatever you desire.
Just remember to Subscribe and Unsubscribe responsibly!
