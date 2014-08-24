Design
======

- Two (more?) different worlds
    - Different art (lighting, background?) in different worlds
    - Different move rules
    - Different collection rules (3 in a row, 3 cluster, different tags, etc.)

- When it switches, you can get a big bonus if you set it up right
    - Nice juicy switch animations
    - Nice juicy combo animations (POW screenshake etc)

First Step: Research Bejeweled (and others):
============================================

Let's make some terms
---------------------

- Break: A combination of tiles break (and give you points)
- Combo: A break sets off more breaks (Bejeweled calls it a cascade)
- Bonus: You get a break larger than the minimum for your ruleset
- Move: You make a move, in Bejeweled, it's swapping 1 for 1 adjacent

Bejeweled Rules
---------------

- 8x8
- Swapping is only 1 direction, and only allowed if it causes a break
- When you drag part way across a square, it tries to swap in that direction
- Bonuses produce new types of tiles
- You can also swap by clicking on two adjacent tiles in order
- Produces callouts with your combo
- Progress bar at the bottom to show level progress
- Shows hints if you haven't made a move in a while

Gems! Rules
-----------

- Hex grid
- Rotate clumps of 3, try to make clumps
- Special pieces

*Notes:* Less pleasant to play than Bejeweled, because:
- Takes away all interaction while combos are going
- Feels too easy/arbitrary, combos feel far more random
- Not enough interaction?
- No warning on the timer, feels like a punishment rather than a motivator
    (In timed Bejeweled the timer is obvious and driving play, here it just kills you)


Apply to Our Design
===================

- 8x8 (?)
