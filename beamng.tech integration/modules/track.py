track = [
    (2000, 2000, 100, 10),  # Start
    (2050, 2000, 100, 10),  # Extended Straight
    (2080, 2005, 100, 10),  # Slight curve
    (2100, 2015, 100, 10),  # Gradual right turn
    (2120, 2030, 100, 10),  # Smooth turn
    (2140, 2050, 100, 10),  # Straight segment
    (2155, 2070, 100, 10),  # Start of left curve
    (2170, 2090, 100, 10),  # Left turn continuation
    (2180, 2120, 100, 10),  # Smooth left curve
    (2190, 2150, 100, 10),  # Straight segment
    (2175, 2180, 100, 10),  # Right turn start
    (2160, 2200, 100, 10),  # Right curve continuation
    (2140, 2220, 100, 10),  # End of right curve
    (2120, 2235, 100, 10),  # Straight segment
    (2090, 2245, 100, 10),  # Left curve start
    (2065, 2240, 100, 10),  # Smooth left curve
    (2040, 2230, 100, 10),  
    (2020, 2220, 100, 10),  # Straight after finish line
    (2000, 2200, 100, 10),  # Entry to first hairpin
    (2020, 2180, 100, 10),  # Exit from first hairpin
    (2050, 2170, 100, 10),  # Straight between hairpins
    (2070, 2150, 100, 10),  # Entry to second hairpin
    (2050, 2130, 100, 10),  # Exit from second hairpin
    (2020, 2120, 100, 10),  # Straight segment
    (2000, 2100, 100, 10),  # Back to flow
]

# Chat GPT interpretation of Monza
"""
track = [
    (2000, 2000, 100, 10),  # Start/Finish Line
    (2050, 2000, 100, 10),  # Slight straight towards the first chicane
    (2100, 2020, 100, 10),  # First chicane entry
    (2150, 2040, 100, 10),  # Exit of the first chicane
    (2200, 2100, 100, 10),  # Long straight (Curva Grande start)
    (2230, 2200, 100, 10),  # Curva Grande end
    (2200, 2300, 100, 10),  # Entry into second chicane
    (2150, 2350, 100, 10),  # Exit of second chicane
    (2100, 2400, 100, 10),  # Lesmo 1 entry
    (2050, 2450, 100, 10),  # Lesmo 1 exit
    (2020, 2500, 100, 10),  # Lesmo 2 entry
    (2000, 2550, 100, 10),  # Lesmo 2 exit
    (1950, 2600, 100, 10),  # Straight leading to Ascari chicane
    (1900, 2570, 100, 10),  # Ascari chicane entry
    (1850, 2520, 100, 10),  # Ascari chicane exit
    (1800, 2450, 100, 10),  # Long straight towards Parabolica
    (1850, 2350, 100, 10),  # Parabolica entry
    (1900, 2250, 100, 10),  # Parabolica midpoint
    (2000, 2150, 100, 10),  # Parabolica exit back to Start/Finish Line
]
track = [
    (2000, 2000, 100, 10),  # Start/Finish Line
    (2050, 2000, 100, 10),  # Straight leading to Tamburello
    (2100, 2020, 100, 10),  # Tamburello chicane entry
    (2150, 2040, 100, 10),  # Tamburello chicane exit
    (2200, 2080, 100, 10),  # Straight to Villeneuve
    (2250, 2120, 100, 10),  # Villeneuve chicane entry
    (2300, 2150, 100, 10),  # Villeneuve chicane exit
    (2350, 2200, 100, 10),  # Tosa hairpin entry
    (2330, 2250, 100, 10),  # Tosa hairpin exit
    (2280, 2300, 100, 10),  # Straight to Piratella
    (2250, 2350, 100, 10),  # Piratella corner
    (2200, 2400, 100, 10),  # Downhill to Acque Minerali
    (2150, 2380, 100, 10),  # Acque Minerali entry
    (2100, 2350, 100, 10),  # Acque Minerali exit
    (2050, 2320, 100, 10),  # Straight to Variante Alta
    (2000, 2300, 100, 10),  # Variante Alta chicane entry
    (1950, 2280, 100, 10),  # Variante Alta chicane exit
    (1900, 2250, 100, 10),  # Rivazza 1
    (1850, 2150, 100, 10),  # Rivazza 2
    (1900, 2100, 100, 10),  # Straight to Start/Finish Line
]
"""
