[System.Serializable]
public class RouteEntry {   
	public IP genmask;
	public IP destination;
	public IP gateway;

	public string iface;
	public string flags; //?
	public int metric;
	public int refe;
	public int use;

	public RouteEntry(IP destination, IP gateway, IP genmask) {
		this.genmask = genmask;
		this.destination = destination;
		this.gateway = gateway;
	}
	public RouteEntry(IP destination, IP gateway, IP genmask, string iface, string flags, int metric, int refe, int use)
		: this(destination, gateway, genmask) {
		this.iface = iface;
		this.flags = flags;
		this.metric = metric;
		this.refe = refe;
		this.use = use;
	}

	public override bool Equals (object obj) {
		RouteEntry comp = obj as RouteEntry;
		return (
			genmask == comp.genmask &&
			destination == comp.destination &&
			gateway == comp.gateway &&
			iface == comp.iface
		);
	}
	public override int GetHashCode () {
		return base.GetHashCode ();
	}
}